using Dfo.Sample.Core.ServiceStack;
using Dfo.Sample.Core.Web.Base;
using Dfo.Sample.Dto.User;
using Dfo.Sample.IBiz.User;
using Dfo.Sample.IBiz.User.Request;
using Dfo.Sample.IBiz.User.Response;
using System.Threading.Tasks;

namespace Dfo.Sample.Api.User.Model
{
    public class CreateUserAction : BaseActionCommand<ICreateUserBiz>
    {
        public CreateUserAction() : base()
        {
        }

        public UserDto RequestData { get; set; }

        public override async Task<BaseServicesResult> ExecuteActionAsync(ISessionContext context)
        {
            var request = new CreateUserRequest
            {
            };
            return await Execute<CreateUserResponse>(request);
        }

        public override void Init()
        {
        }

        public override bool Validate()
        {
            return true;
        }
    }
}