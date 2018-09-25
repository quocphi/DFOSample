using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.RegistrationByConvention;
using Unity.Resolution;

namespace Dfo.Sample.Core.DependencyInjection
{
    public class DependencyInjectorManager : IDependencyInjector, IDisposable
    {
        #region Private Declarations

        private const string ContainerNameKey = "UnityContainerName";
        private const string AppNamespace = "Dfo.Sample";
        private const string DllExtention = ".dll";
        private readonly IUnityContainer _container;
        private DependencyResolver _defaultResolver;
        private Dictionary<Type, HashSet<Type>> _internalTypeMapping;

        #endregion Private Declarations

        #region Constructor

        public DependencyInjectorManager()
        {
            _container = new UnityContainer();

            var section = new UnityConfigurationSection();

            section = (UnityConfigurationSection)ConfigurationManager.GetSection("UnityContainerName");
            // Default container
            section.Configure(_container);
        }

        internal DependencyResolver DefaultResolver
        {
            get
            {
                if (_container == null)
                {
                    throw new Exception("Container is NULL");
                }

                if (_defaultResolver == null)
                {
                    _defaultResolver = new DependencyResolver(_container);
                }

                return _defaultResolver;
            }
        }

        public void Dispose()
        {
            if (_container != null)
            {
                _container.Dispose();
            }
        }

        #endregion Constructor

        #region Public Methods

        public void Register<I, T>()
            where T : I
        {
            _container.RegisterType<I, T>(new ContainerControlledLifetimeManager());
        }

        public void RegisterInstance<T>(T value)
        {
            _container.RegisterInstance<T>(value);
        }

        public void RegisterTypes()
        {
            foreach (var type in GetClassesFromAssemblies())
            {
                var interfacesToBeRegsitered = GetInterfacesToBeRegistered(type);
                AddToInternalTypeMapping(type, interfacesToBeRegsitered);
            }

            RegisterConventions();
        }

        public void Register<I, T>(params object[] parameterValues)
            where T : I
        {
            _container.RegisterType<I, T>(new InjectionConstructor(parameterValues));
        }

        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public T Resolve<T>(string name, params IDependencyParameterExtends[] parameters)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (parameters != null)
                {
                    ResolverOverride[] unityParameters =
                    (from p in parameters
                     select new ParameterOverride(p.ParameterName, p.ParameterValue)).ToArray();
                    return _container.Resolve<T>(name, unityParameters);
                }

                //resolve name only
                return _container.Resolve<T>(name);
            }

            if (parameters != null)
            {
                return ResolveParametersOnly<T>(parameters);
            }

            return Resolve<T>();
        }

        /// <summary>
        /// Resoolve a type while allowing some constructor parameters to be overridden.
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="parameters">The overrides.</param>
        /// <returns>Resolve</returns>
        public T Resolve<T>(params IDependencyParameterExtends[] parameters)
        {
            if (parameters != null)
            {
                return ResolveParametersOnly<T>(parameters);
            }

            return Resolve<T>();
        }

        public T TryResolve<T>(Func<T> defaultFactory = null, params IDependencyParameterExtends[] parameters)
        {
            try
            {
                return Resolve<T>(parameters);
            }
            catch
            {
                // could not resolve. we allow this and don't rethrow.
                return defaultFactory != null ? defaultFactory() : default(T);
            }
        }

        public T TryResolve<T>(string name, Func<T> defaultFactory = null, params IDependencyParameterExtends[] parameters)
        {
            try
            {
                return Resolve<T>(name, parameters);
            }
            catch
            {
                // could not resolve. we allow this and don't rethrow.
                return defaultFactory != null ? defaultFactory() : default(T);
            }
        }

        private T ResolveParametersOnly<T>(params IDependencyParameterExtends[] parameters)
        {
            ResolverOverride[] unityParameters =
            (from p in parameters
             select new ParameterOverride(p.ParameterName, p.ParameterValue)).ToArray();
            return _container.Resolve<T>(unityParameters);
        }

        #endregion Public Methods

        #region Private Methods

        private void RegisterConventions()
        {
            foreach (var typeMapping in _internalTypeMapping)
            {
                if (typeMapping.Value.Count == 1)
                {
                    var type = typeMapping.Value.First();
                    _container.RegisterType(typeMapping.Key, type);
                }
                else
                {
                    foreach (var type in typeMapping.Value)
                    {
                        _container.RegisterType(
                            typeMapping.Key, type, GetNameForRegsitration(type));
                    }
                }
            }
        }

        private IEnumerable<Type> GetClassesFromAssemblies()
        {
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>();
            var allClasses = AllClasses.FromAssemblies(assemblies);

            return allClasses.Where(
                n => n.Namespace != null
                     && n.Namespace.StartsWith(AppNamespace, StringComparison.InvariantCultureIgnoreCase));
        }

        private IEnumerable<Type> GetInterfacesToBeRegistered(Type type)
        {
            var allInterfacesOnType = type.GetInterfaces()
                .Select(i => i.IsGenericType ? i.GetGenericTypeDefinition() : i).ToList();

            return allInterfacesOnType.Except(allInterfacesOnType.SelectMany(i => i.GetInterfaces())).ToList();
        }

        private void AddToInternalTypeMapping(Type type, IEnumerable<Type> interfacesOnType)
        {
            if (_internalTypeMapping == null)
            {
                _internalTypeMapping = new Dictionary<Type, HashSet<Type>>();
            }

            foreach (var interfaceOnType in interfacesOnType)
            {
                if (!_internalTypeMapping.ContainsKey(interfaceOnType))
                {
                    _internalTypeMapping[interfaceOnType] = new HashSet<Type>();
                }

                _internalTypeMapping[interfaceOnType].Add(type);
            }
        }

        private string GetNameForRegsitration(Type type)
        {
            return type.FullName;
        }

        #endregion Private Methods
    }
}