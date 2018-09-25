using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfo.Sample.Core.DependencyInjection
{
    public class DependencyProvider
    {
        #region Private Declarations
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
                            // default to unity if not configured
                            string defaultAssemblyName = typeof(DependencyInjectorManager).Assembly.FullName;
                            string defaultTypeName = typeof(DependencyInjectorManager).FullName;

                            _injector = (IDependencyInjector)AppDomain.CurrentDomain.CreateInstanceAndUnwrap(defaultAssemblyName, defaultTypeName);
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
