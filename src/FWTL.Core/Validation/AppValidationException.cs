using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace FWTL.Core.Validation
{
    public class AppValidationException : ValidationException
    {
        public AppValidationException(IEnumerable<ValidationFailure> errors) : base(errors.ToList())
        {
        }

        public AppValidationException(string property, string message) : base(new List<ValidationFailure>
            {new ValidationFailure(property, message)})
        {
        }
    }
}