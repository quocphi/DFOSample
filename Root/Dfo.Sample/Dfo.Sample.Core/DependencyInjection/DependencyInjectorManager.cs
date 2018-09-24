using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.RegistrationByConvention;
using Unity.Resolution;
using System.Web.Compilation;
using System.Reflection;
using System.IO;

namespace Dfo.Sample.Core.DependencyInjection
{
    public class DependencyInjectorManager: IDependencyInjector, IDisposable
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

        /// <summary>
        /// DependencyInjectorManager
        /// </summary>
        public DependencyInjectorManager()
        {
            _container = new UnityContainer();

            // We can optionally specify a container name in config to get around unity hierarchical config issues
            var section = new UnityConfigurationSection();

            // Default container
            section.Configure(_container);
        }

        /// <summary>
        /// Gets the default resolver.
        /// </summary>
        /// <value>
        /// The default resolver.
        /// </value>
        /// <exception cref="System.Exception">Container must be initialized</exception>
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_container != null)
            {
                _container.Dispose();
            }
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Register.
        /// </summary>
        /// <typeparam name="I">I</typeparam>
        /// <typeparam name="T">T</typeparam>
        public void Register<I, T>()
            where T : I
        {
            _container.RegisterType<I, T>(new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// Register a instance
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="value">Instance</param>
        public void RegisterInstance<T>(T value)
        {
            _container.RegisterInstance<T>(value);
        }

        /// <summary>
        /// RegisterTypes from Loaded Assemblies
        /// </summary>
        public void RegisterTypes()
        {
            foreach (var type in GetClassesFromAssemblies())
            {
                var interfacesToBeRegsitered = GetInterfacesToBeRegistered(type);
                AddToInternalTypeMapping(type, interfacesToBeRegsitered);
            }

            RegisterConventions();
        }

        /// <summary>
        /// Register with param.
        /// </summary>
        /// <typeparam name="I">I</typeparam>
        /// <typeparam name="T">T</typeparam>
        /// <param name="parameterValues">parameterValues</param>
        public void Register<I, T>(params object[] parameterValues)
            where T : I
        {
            _container.RegisterType<I, T>(new InjectionConstructor(parameterValues));
        }

        /// <summary>
        /// Resolve a type. Returns new instance of supplied type if not overridden.
        /// </summary>
        /// <param name="type">desired type</param>
        /// <returns>
        /// New instance of configured type implementing T
        /// </returns>
        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        /// <summary>
        /// Resolve a type. Returns new instance of supplied type if not overridden.
        /// </summary>
        /// <typeparam name="T">desired type</typeparam>
        /// <returns>
        /// New instance of configured type implementing T
        /// </returns>
        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        /// <summary>
        /// Resolves the specified name.
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>Resolve</returns>
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

        /// <summary>
        /// Attempt to resolve a type. No exception will be thrown if the type cannot be instantiated - instead a default
        /// factory
        /// will be invoked (if supplied), otherwise default(T) will be returned.
        /// The expected use is that for most calls (e.g. outside unit tests) no configured type will be defined, so the
        /// factory would be invoked.
        /// </summary>
        /// <typeparam name="T">desired type</typeparam>
        /// <param name="defaultFactory">optional factory operation to create default instance, or null</param>
        /// <param name="parameters">The overrides.</param>
        /// <returns>configured implementor of T, result of factory if supplied, or default(T)</returns>
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

        /// <summary>
        /// Attempt to resolve a type by name. No exception will be thrown if the type cannot be instantiated - instead a
        /// default factory
        /// will be invoked (if supplied), otherwise default(T) will be returned.
        /// The expected use is that for most calls (e.g. outside unit tests) no configured type will be defined, so the
        /// factory would be invoked.
        /// </summary>
        /// <typeparam name="T">desired type</typeparam>
        /// <param name="name">The name.</param>
        /// <param name="defaultFactory">optional factory operation to create default instance, or null</param>
        /// <param name="parameters">The overrides.</param>
        /// <returns>
        ///     configured implementor of T, result of factory if supplied, or default(T)
        /// </returns>
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

        /// <summary>
        /// Only resolve Parameters
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="parameters">IDependencyParameterOverride[]</param>
        /// <returns>T</returns>
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
