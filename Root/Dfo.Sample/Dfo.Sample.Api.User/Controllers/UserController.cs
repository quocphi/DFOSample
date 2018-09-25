using Dfo.Sample.Api.User.Model;
using Dfo.Sample.Core.ServiceStack.HttpResult;
using Dfo.Sample.Core.Web.Controllers;
using System.Web.Http;

namespace Dfo.Sample.Api.User.Controllers
{
    public class UserController : BaseApiController
    {
        [HttpPost]
        public HttpActionResult CreateUser([FromBody] CreateUserAction action)
        {
            var result = action.ExecuteActionAsync(SessionContext);
            return new HttpActionResult(result.Result, Request);
        }
    }
}