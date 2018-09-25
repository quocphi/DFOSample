using Dfo.Sample.Core.Message;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace Dfo.Sample.Core.Web.Configurations
{
    public class GlobalExceptionHandler: ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = ExcuteExceptionHandler(context);
        }

        private TextPlainErrorResult ExcuteExceptionHandler(ExceptionHandlerContext context)
        {
            TextPlainErrorResult result;

            if (context.Exception.InnerException != null)
            {
                result = new TextPlainErrorResult
                {
                    Request = context.ExceptionContext.Request,
                    Content = "Dfo ex!",
                    StatusCode = context.Exception.InnerException.Data["StatusCode"] != null ? ((HttpStatusCode)context.Exception.InnerException.Data["StatusCode"]) : HttpStatusCode.InternalServerError,
                    MessageTitle = "Exception!!!",
                    MessageId = "DFO001",
                    FieldName = "DFO002"
                };
            }
            else
            {
                result = new TextPlainErrorResult
                {
                    Request = context.ExceptionContext.Request,
                    Content = string.Format("Server Exception: {0}, {1}", context.Exception.Message, context.Exception.StackTrace),
                    StatusCode = HttpStatusCode.InternalServerError,
                    MessageTitle = "Dfo ex!",
                    MessageId = "M003",
                    FieldName = "DFO003"
                };
            }

            return result;
        }

        private class TextPlainErrorResult : IHttpActionResult
        {
            public HttpRequestMessage Request { get; set; }

            public HttpStatusCode StatusCode { get; set; }

            public string Content { get; set; }

            public string MessageTitle { get; set; }

            public string MessageId { get; set; }

            public string FieldName { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response = Request.CreateResponse(
                    StatusCode,
                    new { Data = new { Result = new BaseResponse() { Success = false, Errors = new ServiceErrors() { new ServiceError() { Message = Content, ErrorCode = ((int)StatusCode).ToString(), Description = MessageTitle, MessageId = MessageId, FieldName = FieldName } } } } });
                response.RequestMessage = Request;

                //call audit and trace
                return Task.FromResult(response);
            }
        }
    }
}
