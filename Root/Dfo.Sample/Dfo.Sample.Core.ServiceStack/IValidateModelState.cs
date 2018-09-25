using FluentValidation.Results;

namespace Dfo.Sample.Core.ServiceStack
{
    public interface IValidateModelState
    {
        ValidationResult ValidateResult { get; set; }
    }
}