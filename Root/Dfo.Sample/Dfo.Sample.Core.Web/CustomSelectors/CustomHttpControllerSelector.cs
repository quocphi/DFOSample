using Dfo.Sample.Core.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Dfo.Sample.Core.Web.CustomSelectors
{
    public class CustomHttpControllerSelector : DefaultHttpControllerSelector
    {
        private readonly HttpConfiguration _configuration;
        private ICollection<Assembly> _assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomHttpControllerSelector"/> class.
        /// </summary>
        /// <param name="configuration">configuration</param>
        public CustomHttpControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// SelectController
        /// </summary>
        /// <param name="request">HttpRequestMessage</param>
        /// <returns>HttpControllerDescriptor</returns>
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            HttpControllerDescriptor controller = null;
            RegisterExternalAssembliesNotAPI();

            string controllerName = GetControllerName(request);

            try
            {
                controller = base.SelectController(request);
                controller = RegisterController(controllerName, controller);
            }
            catch
            {
                controller = RegisterController(controllerName, controller);
            }

            DependencyProvider.Current.RegisterTypes();
            return controller;
        }

        /// <summary>
        /// RegisterExternalAssembliesNotAPI
        /// </summary>
        private void RegisterExternalAssembliesNotAPI()
        {
            foreach (var assembly in LoadExternalAssemblies())
            {
                _assemblies.Add(assembly);
            }
        }

        /// <summary>
        /// LoadExternalAssemblies
        /// </summary>
        /// <returns>IEnumerable<Assembly></returns>
        private IEnumerable<Assembly> LoadExternalAssemblies()
        {
            var pluginDirectory = AppDomain.CurrentDomain.BaseDirectory + "PLG";
            if (!Directory.Exists(pluginDirectory))
            {
                Directory.CreateDirectory(pluginDirectory);
            }

            var files = Directory.GetFiles(pluginDirectory, "*.dll");
            return files.Select(Assembly.LoadFrom);
        }

        /// <summary>
        /// RegisterController
        /// </summary>
        /// <param name="controllerName">string</param>
        /// <param name="controller">HttpControllerDescriptor</param>
        /// <returns>HttpControllerDescriptor</returns>
        private HttpControllerDescriptor RegisterController(string controllerName, HttpControllerDescriptor controller)
        {
            foreach (var assembly in LoadExternalAssemblies())
            {
                Type controllerType = assembly.GetTypes().FirstOrDefault(i => typeof(IHttpController).IsAssignableFrom(i) && i.Name.ToLower() == controllerName.ToLower() + "controller");
                if (controllerType != null)
                {
                    return new HttpControllerDescriptor(_configuration, controllerName, controllerType);
                }
            }

            return controller;
        }
    }
}