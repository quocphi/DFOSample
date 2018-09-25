using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace Dfo.Sample.Core.Web.Configurations
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "Api",
                routeTemplate: "{controller}/{action}");

            SetJsonFormatter(config);
        }

        /// <summary>
        /// Config Json formater
        /// </summary>
        /// <param name="config">HttpConfiguration</param>
        private static void SetJsonFormatter(HttpConfiguration config)
        {
            var formatters = config.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
