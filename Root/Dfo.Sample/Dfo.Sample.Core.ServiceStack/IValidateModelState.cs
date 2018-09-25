using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfo.Sample.Core.ServiceStack
{
    public interface IValidateModelState
    {
        ValidationResult ValidateResult { get; set; }
    }
}
