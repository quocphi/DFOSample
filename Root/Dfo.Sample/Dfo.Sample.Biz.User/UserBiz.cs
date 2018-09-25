using Dfo.Sample.Core;
using Dfo.Sample.Core.ServiceStack;
using Dfo.Sample.Dto.User;
using Dfo.Sample.Dto.User.Request;
using Dfo.Sample.Dto.User.Response;
using Dfo.Sample.DummyData;
using Dfo.Sample.IBiz.User;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dfo.Sample.Biz.User
{
    public class UserBiz: BizCommand, IUserBiz
    {
        public UserBiz()
        { 
            //Do nothing at this moment
        }

        /// <summary>
        /// CreateUser's Logic
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CreateUserResponse> CreateUser(CreateUserRequest request)
        {
            Random rnd = new Random();
            int rndNumber = rnd.Next(1, 10000);

            UsersData.UsersDataInMem.Add(new UserDto
            {
                Id = rndNumber,
                Name = request.RequestData.Name,
                Age = request.RequestData.Age,
                Address = request.RequestData.Address.ToString()
            });

            var response = new CreateUserResponse()
            {
                Success = true,
            };

            return await Task.FromResult(response);
        }

        /// <summary>
        /// UpdateUser's Logic
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request)
        {
            UserDto theUser = UsersData.UsersDataInMem.FirstOrDefault(x => x.Id == request.RequestData.Id);
            theUser.Name = request.RequestData.Name;
            theUser.Age = request.RequestData.Age;
            theUser.Address = request.RequestData.Address;
            var response = new UpdateUserResponse()
            {
                Success = true,
                Data = theUser
            };

            return await Task.FromResult(response);
        }

        /// <summary>
        /// GetUsers's Logic
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetUsersResponse> GetUsers(GetUsersRequest request)
        {
            var response = new GetUsersResponse()
            {
                Success = true,
                Data= UsersData.UsersDataInMem.ToList()
            };

            return await Task.FromResult(response);
        }

        /// <summary>
        /// GetUserById's Logic
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request)
        {
            UserDto theUser = UsersData.UsersDataInMem.FirstOrDefault(x => x.Id == request.RequestData.Id);
            var response = new GetUserByIdResponse()
            {
                Success = true,
                Data = theUser
            };
            return await Task.FromResult(response);
        }
    }
}