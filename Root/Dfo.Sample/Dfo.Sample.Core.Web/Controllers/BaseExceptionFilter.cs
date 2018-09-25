using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Dfo.Sample.Core.Web.Controllers
{
    public class BaseExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            // TODO
            base.OnException(actionExecutedContext);
        }
    }
}
