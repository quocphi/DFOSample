using Dfo.Sample.Api.User.Model;
using Dfo.Sample.Core.Validator;
using FluentValidation;

namespace Dfo.Sample.Api.User.Validators
{
    public class UpdateUserActionValidator : BaseValidatorProvider<UpdateUserAction>
    {
        public UpdateUserActionValidator()
        {
            //The Model for required not NULL
            RuleFor(o => o.RequestData).NotNull().WithMessage("The Model is required!");
            RuleFor(o => o.RequestData.Id).NotNull().WithMessage("Field is required!");
            RuleFor(o => o.RequestData.Name).NotNull().WithMessage("Field is required!");
            RuleFor(o => o.RequestData.Age).NotNull().WithMessage("Field is required!");
            RuleFor(o => o.RequestData.Address).NotNull().WithMessage("Field is required!");

            //The Model with string property for required max lenghth
            RuleFor(o => o.RequestData.Name).MaximumLength(50).WithMessage("Max lenght of string is 50!");
            RuleFor(o => o.RequestData.Address).MaximumLength(50).WithMessage("Max lenght of string is 50!");
        }
    }
}