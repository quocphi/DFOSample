using System.Web.Http.Filters;

namespace Dfo.Sample.Core.Web.Controllers
{
    public class BaseExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);
        }
    }
}