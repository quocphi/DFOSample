using Dfo.Sample.Core.ServiceStack;
using Dfo.Sample.Core.Web.Base;
using Dfo.Sample.Dto.User;
using Dfo.Sample.IBiz.User;
using Dfo.Sample.Dto.User.Request;
using Dfo.Sample.Dto.User.Response;
using System.Threading.Tasks;
using Dfo.Sample.Api.User.Validators;

namespace Dfo.Sample.Api.User.Model
{
    public class UpdateUserAction : BaseActionCommand<IUserBiz>
    {
        public UserDto RequestData { get; set; }

        public override async Task<BaseServicesResult> ExecuteActionAsync(ISessionContext context)
        {
            var request = new UpdateUserRequest
            {
                RequestData = RequestData
            };
            return await Execute<UpdateUserResponse>(request);
        }

        public override void Init()
        {
        }

        public override bool Validate()
        {
            ValidateResult = new UpdateUserActionValidator().Validate(this);
            return ValidateResult.IsValid;
        }
    }
}