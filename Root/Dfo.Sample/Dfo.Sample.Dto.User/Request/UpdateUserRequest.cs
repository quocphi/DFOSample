using Dfo.Sample.Core.Message;

namespace Dfo.Sample.Dto.User.Request
{
    public class UpdateUserRequest : BaseRequest
    {
        public UserDto RequestData { get; set; }

    }
}