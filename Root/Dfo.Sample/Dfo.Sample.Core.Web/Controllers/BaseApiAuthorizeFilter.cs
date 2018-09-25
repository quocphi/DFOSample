using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Dfo.Sample.Core.Web.Controllers
{
    public class BaseApiAuthorizeFilter: AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return base.IsAuthorized(actionContext);
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
        }
    }
}
