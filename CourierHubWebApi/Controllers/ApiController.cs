using Azure.Core;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CourierHubWebApi.Controllers
{
    public class ApiController: ControllerBase
    {
        public ModelStateDictionary? Validate<T>(IValidator<T> validator, T validatee)
        {
            ValidationResult validationResult = validator.Validate(validatee);
            if (!validationResult.IsValid)
            {
                ModelStateDictionary modelStateDictionary = new();

                foreach (ValidationFailure failure in validationResult.Errors)
                {
                    modelStateDictionary.AddModelError(
                        failure.PropertyName,
                        failure.ErrorMessage);
                }

                return modelStateDictionary;
            }
            return null;
        }
    }
}
