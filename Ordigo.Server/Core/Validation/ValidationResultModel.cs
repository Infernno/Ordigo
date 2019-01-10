using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ordigo.Server.Core.Validation
{
    public class ValidationResultModel : List<ValidationError>
    {
        public ValidationResultModel(ModelStateDictionary modelState)
        {
            var errors = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                .ToList();

            foreach (var error in errors)
            {
                Add(error);
            }
        }
    }
}
