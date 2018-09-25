using Dfo.Sample.Api.User.Model;
using Dfo.Sample.Core.Validator;
using FluentValidation;

namespace Dfo.Sample.Api.User.Validators
{
    public class CreateUserActionValidator : BaseValidatorProvider<CreateUserAction>
    {
        public CreateUserActionValidator()
        {
            RuleFor(o => o.RequestData.Age).NotNull();
        }
    }
}