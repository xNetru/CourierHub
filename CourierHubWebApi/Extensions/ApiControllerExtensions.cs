using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CourierHubWebApi.Extensions {

    public static class ApiControllerExtensions {
        private static readonly string _serviceIdItemsIndex = "ServiceIdIndex";
        public static ModelStateDictionary? Validate<T>(this ControllerBase controller, IValidator<T> validator, T validatee) {
            ValidationResult validationResult = validator.Validate(validatee);
            if (!validationResult.IsValid) {
                ModelStateDictionary modelStateDictionary = new();

                foreach (ValidationFailure failure in validationResult.Errors) {
                    modelStateDictionary.AddModelError(
                        failure.PropertyName,
                        failure.ErrorMessage);
                }

                return modelStateDictionary;
            }
            return null;
        }
        public static ErrorOr<int> ExtractServiceIdFromContext(this ControllerBase controller) {
            string? serviceIdItemsIndex = controller.HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetValue<string>(_serviceIdItemsIndex);
            if (serviceIdItemsIndex != null) {
                if (controller.HttpContext.Items.TryGetValue(serviceIdItemsIndex, out object? stringServiceId)) {
                    if (stringServiceId is string s) {
                        if (int.TryParse(s, out int serviceId)) {
                            // In case of errors inside _inquireService the information about them is not passed
                            // TODO: passing errors
                            return serviceId;
                        }
                    }
                }
            }
            return Error.Custom(description: "Middleware error", type: (int)ErrorType.Unexpected, code: "500");
        }
    }
}
