using Dfo.Sample.Api.User.Model;
using Dfo.Sample.Core.Validator;
using FluentValidation;

namespace Dfo.Sample.Api.User.Validators
{
    public class GetUsersActionValidator : BaseValidatorProvider<GetUsersAction>
    {
        public GetUsersActionValidator()
        {
        }
    }
}