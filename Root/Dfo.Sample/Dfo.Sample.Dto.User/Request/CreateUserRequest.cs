﻿using Dfo.Sample.Core.Message;

namespace Dfo.Sample.Dto.User.Request
{
    public class CreateUserRequest: BaseRequest
    {
        public UserDto RequestData { get; set; }
    }
}