using Dfo.Sample.Core.Web.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
