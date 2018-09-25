using System.Web.Http.Dependencies;
using Unity;

namespace Dfo.Sample.Core.DependencyInjection
{
    public class DependencyResolver : DependencyScope, IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnityDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">container</param>
        public DependencyResolver(IUnityContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// BeginScope
        /// </summary>
        /// <returns>IDependencyScope</returns>
        public IDependencyScope BeginScope()
        {
            IUnityContainer childContainer = Container.CreateChildContainer();

            return new DependencyScope(childContainer);
        }
    }
}