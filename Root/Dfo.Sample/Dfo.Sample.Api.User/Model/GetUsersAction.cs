using Dfo.Sample.Api.User.Validators;
using Dfo.Sample.Core.ServiceStack;
using Dfo.Sample.Core.Web.Base;
using Dfo.Sample.Dto.User.Response;
using Dfo.Sample.IBiz.User;
using System.Threading.Tasks;

namespace Dfo.Sample.Api.User.Model
{
    public class GetUsersAction : BaseActionCommand<IUserBiz>
    {
        public string RequestData { get; set; }

        public override async Task<BaseServicesResult> ExecuteActionAsync(ISessionContext context)
        {
            return await Execute<GetUsersResponse>(RequestData);
        }

        public override void Init()
        {
        }

        public override bool Validate()
        {
            ValidateResult = new GetUsersActionValidator().Validate(this);
            return ValidateResult.IsValid;
        }
    }
}