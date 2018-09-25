﻿using Dfo.Sample.Core.ServiceStack;
using Dfo.Sample.Core.Web.Base;
using Dfo.Sample.Dto.User;
using Dfo.Sample.IBiz.User;
using Dfo.Sample.Dto.User.Request;
using Dfo.Sample.Dto.User.Response;
using System.Threading.Tasks;
using Dfo.Sample.Api.User.Validators;

namespace Dfo.Sample.Api.User.Model
{
    public class CreateUserAction : BaseActionCommand<IUserBiz>
    {
        public UserDto RequestData { get; set; }

        public override async Task<BaseServicesResult> ExecuteActionAsync(ISessionContext context)
        {
            var request = new CreateUserRequest
            {
                RequestData = RequestData
            };
            return await Execute<CreateUserResponse>(request);
        }

        public override void Init()
        {
        }

        public override bool Validate()
        {
            ValidateResult = new CreateUserActionValidator().Validate(this);
            return ValidateResult.IsValid;
        }
    }
}