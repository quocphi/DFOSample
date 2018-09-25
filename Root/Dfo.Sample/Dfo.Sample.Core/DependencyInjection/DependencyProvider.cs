using System;

namespace Dfo.Sample.Core.DependencyInjection
{
    public class DependencyProvider
    {
        #region Private Declarations
        private const string DependencyInjectorImplAssemblyKey = "DependencyInjectorImplAssembly";
        private const string DependencyInjectorImplTypeKey = "DependencyInjectorImplType";
        private static readonly object SInjectorLock = new object();
        private static IDependencyInjector _injector;

        #endregion Private Declarations

        #region Public Methods

        /// <summary>
        /// Gets retrieve the configured dependency injector implementation.
        /// </summary>
        public static IDependencyInjector Current
        {
            get
            {
                if (_injector == null)
                {
                    lock (SInjectorLock)
                    {
                        if (_injector == null)
                        {
                            string defaultAssemblyName = typeof(DependencyInjectorManager).Assembly.FullName;
                            string defaultTypeName = typeof(DependencyInjectorManager).FullName;

                            string configuredAssemblyName = DfoUtils.GetStringKey(DependencyInjectorImplAssemblyKey, string.Empty);
                            string configuredTypeName = DfoUtils.GetStringKey(DependencyInjectorImplTypeKey, string.Empty);

                            string assemblyName = string.IsNullOrEmpty(configuredAssemblyName)
                                ? defaultAssemblyName
                                : configuredAssemblyName;
                            string typeName = string.IsNullOrEmpty(configuredTypeName)
                                ? defaultTypeName
                                : configuredTypeName;

                            _injector = (IDependencyInjector)AppDomain.CurrentDomain.CreateInstanceAndUnwrap(assemblyName, typeName);
                        }
                    }
                }

                return _injector;
            }
        }

        #endregion Public Methods

        /// <summary>
        /// Gets the default resolver.
        /// </summary>
        /// <value>
        /// The default resolver.
        /// </value>
        public static DependencyResolver DefaultResolver
        {
            get
            {
                var injector = Current as DependencyInjectorManager;
                return injector?.DefaultResolver;
            }
        }
    }
}