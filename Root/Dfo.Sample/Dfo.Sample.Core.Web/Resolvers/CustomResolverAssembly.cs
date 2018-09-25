using System.Collections.Generic;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace Dfo.Sample.Core.Web.Resolvers
{
    public class CustomResolverAssembly : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            return base.GetAssemblies();
        }
    }
}