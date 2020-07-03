using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace FWTL.Common.Extensions
{
    public static class IdentityResultExtensions
    {
        public static IEnumerable<ValidationFailure> GetErrors(this IdentityResult @this, string propertyName = nameof(IdentityResult))
        {
            return @this.Errors.Select(error => new ValidationFailure(propertyName, error.Description));
        }
    }
}