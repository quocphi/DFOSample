using Dfo.Sample.Core.Web.Base;
using System.Web.Http;

namespace Dfo.Sample.Core.Web.Controllers
{
    /// <summary>
    /// BaseApiController
    /// </summary>
    [BaseApiAuthorizeFilter]
    [BaseExceptionFilter]
    [BaseApiActionFilter]
    public abstract class BaseApiController : ApiController
    {
        protected BaseApiController()
        {
        }

        public ISessionContext SessionContext { get; set; }
    }
}