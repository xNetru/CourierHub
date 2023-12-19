using CourierHubWebApi.Models;
using CourierHubWebApi.Services.Contracts;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CourierHubWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class InquireController : ControllerBase {
        private IInquireService _inquireService;
        private static readonly string _serviceIdItemsIndex = "ServiceIdIndex";
        public InquireController(IInquireService inquireService) {
            _inquireService = inquireService;
        }
        [HttpPost]
        public IActionResult CreateInquire(CreateInquireRequest request,
            [FromServices] IValidator<CreateInquireRequest> validator) {
            ValidationResult validationResult = validator.Validate(request);
            if (!validationResult.IsValid) {
                ModelStateDictionary modelStateDictionary = new();

                foreach (ValidationFailure failure in validationResult.Errors) {
                    modelStateDictionary.AddModelError(
                        failure.PropertyName,
                        failure.ErrorMessage);
                }

                return ValidationProblem(modelStateDictionary);
            }

            string? serviceIdItemsIndex = HttpContext.RequestServices.GetRequiredService<IConfiguration>().GetValue<string>(_serviceIdItemsIndex);
            if (serviceIdItemsIndex != null) {
                if (HttpContext.Items.TryGetValue(serviceIdItemsIndex, out object? stringServiceId)) {
                    if (stringServiceId is string s) {
                        if (int.TryParse(s, out int serviceId)) {
                            // In case of errors inside _inquireService the information about them is not passed
                            // TODO: passing errors
                            return _inquireService.CreateInquire(request, serviceId).Result.Match(
                                response => CreatedAtAction(
                                    actionName: nameof(CreateInquire),
                                    routeValues: new { id = response.Code },
                                    value: response),
                                errors => Problem());
                        }
                    }

                }

            }

            return Problem();


        }
    }
}
