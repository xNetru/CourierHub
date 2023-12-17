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
        public InquireController(IInquireService inquireService) {
            _inquireService = inquireService;
        }
        [HttpPost]
        public IActionResult CreateInquire(CreateInquireRequest request, [FromServices] IValidator<CreateInquireRequest> validator) {
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

            return _inquireService.CreateInquire(request).Result.Match(
                inquire => CreatedAtAction(
                    actionName: nameof(CreateInquire),
                    routeValues: new { id = inquire.Id },
                    value: new CreateInquireResponse(inquire.Id)),
                errors => Problem());

        }
    }
}
