
using Dfo.Sample.Core.DependencyInjection;
using FluentValidation.Results;
using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace Dfo.Sample.Core.ServiceStack
{
    /// <summary>
    /// BaseActionCommand for whole implementation on the Action classes.
    /// </summary>
    /// <typeparam name="TBusiness">IBusinessLogic</typeparam>E:\Works\M35.Source\Root\DS\Mjs.AW.CommonWeb-DS\Mjs.AW.Common.Web.API\Actions\BaseActionCommand.cs
    public abstract class BaseActionCommand<TBusiness> : BaseCommand<TBusiness>, ICommandAction, IValidateModelState
        where TBusiness : class
    {
        #region IValidateModelState Implementation

        public ValidationResult ValidateResult { get; set; }

        #endregion IValidateModelState Implementation

        #region Public Properties

        /// <summary>
        /// Gets or sets Browser Session Id
        public string BrowserSessionId { get; set; }

        /// <summary>
        /// Gets or sets UserLogin
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        /// Gets or sets company code This information will be populated in OnExcecuting of ActionFilter
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// Gets or sets Fiscal Year This infomation will be populated in OnExcecuting of ActionFilter
        /// </summary>
        public int FiscalYear { get; set; }

        #endregion Public Properties

        #region Private Properties

        /// <summary>
        /// Business Logic Type
        /// </summary>
        private readonly Type _businessLogicType;

        /// <summary>
        /// Instance of Business Logic
        /// </summary>
        private readonly TBusiness _businessLogic;

        #endregion Private Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseActionCommand{TBusiness}"/> class. BaseActionCommand
        /// </summary>
        protected BaseActionCommand()
        {
            _businessLogicType = typeof(TBusiness);
            _businessLogic = DependencyProvider.Current.Resolve<TBusiness>();
        }

        #endregion Constructors

        #region Public Methods

        /// <summary> ExecuteAction </summary> <param name="context">IDataContext</param> <returns>Task<BaseServicesResult></returns>
        public abstract Task<BaseServicesResult> ExecuteAction(IDataContext context);

        /// <summary>
        /// Invokes Business Logic's method by name
        /// </summary>
        /// <typeparam name="TResponse">Method responese</typeparam>
        /// <param name="methodArgs">Parameters's method</param>
        /// <returns>Response's method</returns>
        public async Task<BaseServicesResult> Execute<TResponse>(params object[] methodArgs)
            where TResponse : BaseResponse, new()
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            TResponse result = default(TResponse);

            try
            {
                Init();

                if (Validate())
                {
                    result = await OnExecute<TResponse>(methodArgs);

                    if (result.Success == false)
                    {
                        ServiceError error = result.Errors.FirstOrDefault();
                        statusCode = error.ErrorCode.IsNullEmpty() ? HttpStatusCode.InternalServerError : (HttpStatusCode)error.ErrorCode.ToInt32();
                    }
                }
                else
                {
                    statusCode = HttpStatusCode.BadRequest;
                    result = new TResponse { Errors = new ServiceErrors() };

                    foreach (var item in ValidateResult.Errors)
                    {
                        result.Errors.Add(new ServiceError()
                        {
                            FieldName = item.PropertyName,
                            Params = item.GetParamMessage(),
                            MessageId = !string.IsNullOrEmpty(item.ResourceName) ? item.GetMessageID() : item.ErrorMessage
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                var expDetail = BusinessLogicExceptionHandling.GetExceptionInfo(ex);
                statusCode = expDetail.ErrorCode.IsEquals(0) ? HttpStatusCode.InternalServerError : (HttpStatusCode)expDetail.ErrorCode;
                result.Errors = new ServiceErrors
                {
                    new ServiceError()
                    {
                        ErrorCode = expDetail.ErrorCode.ToString(),
                        FieldName = expDetail.FieldName,
                        Message = expDetail.FullMessage,
                        MessageId = !string.IsNullOrEmpty(expDetail.MessageId) ? expDetail.MessageId : "M003",
                    }
                };
            }

            return new BaseServicesResult
            {
                ReturnCode = statusCode,
                ReturnData = new BaseResponseHttpActionResult<TResponse>() { Result = result }
            };
        }

        public TRequest ToRequest<TRequest>()
            where TRequest : BaseRequest, new()
        {
            var request = new TRequest()
            {
            };

            return request;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Invokes Business Logic's method by name
        /// </summary>
        /// <typeparam name="TResponse">Method responese</typeparam>
        /// <param name="methodArgs">Parameters's method</param>
        /// <returns>Response's method</returns>
        private async Task<TResponse> OnExecute<TResponse>(params object[] methodArgs)
            where TResponse : BaseResponse, new()
        {
            TResponse response = default(TResponse);

            //get medthod name
            string methodName = string.Empty;
            string nameAction = GetType().Name;
            if (!string.IsNullOrEmpty(nameAction))
            {
                methodName = nameAction.Substring(0, nameAction.Length - 6);
            }

            MethodBase method = _businessLogicType.GetMethod(methodName);

                try
                {
                    dynamic awaitable = method.Invoke(_businessLogic, methodArgs);

                    await awaitable;
                    response = (TResponse)awaitable.GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    //Need to consider whether we need to throw the App_Err
                    response = new TResponse()
                    {
                        Success = false,
                        Errors = new ServiceErrors()
                        {
                            new ServiceError()
                            {
                                ErrorCode = expDetail.ErrorCode.ToString(),
                                MessageId = !string.IsNullOrEmpty(expDetail.MessageId) ? expDetail.MessageId : "M003",
                                FieldName = expDetail.FieldName,
                                Message = expDetail.FullMessage
                            }
                        }
                    };
                }
            }

            return response;
        }

        #endregion Private Methods
    }
}