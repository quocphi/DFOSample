using Dfo.Sample.Core.ServiceStack;
using Dfo.Sample.Dto.User.Request;
using Dfo.Sample.Dto.User.Response;
using Dfo.Sample.IBiz.User;
using System.Threading.Tasks;

namespace Dfo.Sample.Biz.User
{
    public class UserBiz: BizCommand, IUserBiz
    {
        public UserBiz()
        {
        }
        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            var response = new CreateUserResponse()
            {
                Success = true,
            };

            return await Task.FromResult(response);
        }
    }
}