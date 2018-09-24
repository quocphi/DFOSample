using Dfo.Sample.Core.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfo.Sample.Core.ServiceStack
{
    public abstract class BaseActionCommand<TBusiness> : BaseCommand<TBusiness>, ICommandAction where TBusiness : class
    {
        public TBusiness BusinessLogic { get; private set; }

        protected BaseActionCommand()
        {
            //Do something
        }
        public abstract Task<BaseServicesResult> ExecuteAction(IDataContext context);

        public async Task<BaseServicesResult> Execute<TResponse>(params object[] methodArgs)
            where TResponse : BaseResponse, new()
        {
            // Init base request data
            BaseRequest request = ToRequest<BaseRequest>();

            // Init business logic and action meta data
            BusinessLogic = DependencyProvider.Current.Resolve<TBusiness>();
           

            // Custom init from action
            Init();

            // Init action utilities and logger

            // Init default response object (equal NULL)
            TResponse response;

            if (Validate())
            {
                response = await OnExecute<TResponse>(methodArgs);
            }
            else
            {
                response = new TResponse { Errors = actionUtility.GetErrors(ValidateResult), Success = false };
            }

            // Build and return service message
            var serviceResult = new BaseServicesResult
            {
                ReturnCode = statusUtility.GetStatusCode(response),
                ReturnData = new BaseResponseHttpActionResult<TResponse>() { Result = response, InternalCode = statusUtility.GetInternalCode(response).ToInt32() }
            };
            return serviceResult;
        }

        private T ToRequest<T>()
        {
            throw new NotImplementedException();
        }
    }
}
