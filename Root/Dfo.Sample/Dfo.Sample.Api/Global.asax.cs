using Dfo.Sample.Core.DependencyInjection;
using Dfo.Sample.Core.Web.Configurations;
using System.Web.Http;

namespace Dfo.Sample.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.DependencyResolver = DependencyProvider.DefaultResolver;
            DependencyProvider.Current.RegisterTypes();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
