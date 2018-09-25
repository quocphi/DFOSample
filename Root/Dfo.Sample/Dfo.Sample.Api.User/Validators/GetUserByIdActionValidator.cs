using Dfo.Sample.Api.User.Model;
using Dfo.Sample.Core.Validator;
using FluentValidation;

namespace Dfo.Sample.Api.User.Validators
{
    public class GetUserByIdActionValidator : BaseValidatorProvider<GetUserByIdAction>
    {
        public GetUserByIdActionValidator()
        {
            //The Model for required not NULL
            RuleFor(o => o.RequestData).NotNull().WithMessage("The Model is required!");
            RuleFor(o => o.RequestData.Id).NotNull().WithMessage("Field is required!");

            //The Model with string property for required max lenghth
            RuleFor(o => o.RequestData.Id).LessThan(2147483647).WithMessage("Max number is 2147483647!");
        }
    }
}