using CourierHubWebApi.Errors;
using CourierHubWebApi.Services.Contracts;
using ErrorOr;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OneOf;

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
        public static OneOf<int, ApiError> GetServiceIdFromHttpContext(this ControllerBase controller,
            IApiKeyService apiKeyService) {
            if (apiKeyService.TryExtractApiKey(controller.HttpContext, out string apiKey)) {
                if (apiKeyService.TryGetServiceId(apiKey, out int serviceId)) {
                    return serviceId;
                }
                return new ApiError(StatusCodes.Status401Unauthorized, "Invalid API key.", "Unauthorized.");
            }
            return new ApiError(StatusCodes.Status401Unauthorized, "Api key was not provided.", "Unauthorized.");
        }
    }
}
