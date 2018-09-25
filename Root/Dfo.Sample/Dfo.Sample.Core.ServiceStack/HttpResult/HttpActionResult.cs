using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Dfo.Sample.Core.ServiceStack.HttpResult
{
    public class HttpActionResult : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        private readonly BaseServicesResult _result;

        public HttpActionResult(BaseServicesResult result, HttpRequestMessage request)
        {
            _result = result;
            _request = request;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = _request.CreateResponse(_result.ReturnCode, _result.ReturnData);

            if (_result.ReturnData == null)
            {
                response = _request.CreateResponse(HttpStatusCode.NotFound);
            }

            response.RequestMessage = _request;

            return await Task.FromResult(response);
        }
    }
}
