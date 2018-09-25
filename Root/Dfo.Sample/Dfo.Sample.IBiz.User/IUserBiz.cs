using Dfo.Sample.Dto.User.Request;
using Dfo.Sample.Dto.User.Response;
using System.Threading.Tasks;

namespace Dfo.Sample.IBiz.User
{
    public interface IUserBiz
    {
        Task<CreateUserResponse> CreateUser(CreateUserRequest request);

        Task<GetUsersResponse> GetUsers(GetUsersRequest request);

        Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request);

        Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request);
    }
}