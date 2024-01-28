using CourierHubWebApi.Extensions;
using CourierHubWebApi.Models;
using CourierHubWebApi.Services.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CourierHubWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class InquireController : ControllerBase {
        private IInquireService _inquireService;
        public InquireController(IInquireService inquireService) {
            _inquireService = inquireService;
        }

        /// <summary>
        /// Creates offer
        /// </summary>
        /// <param name="request">Inquiry</param>
        /// <param name="validator">Validator</param>
        /// <param name="apiKeyService">ApiKeyService</param>
        /// <param name="logger">Logger</param>
        /// <returns>Newly created offer</returns>
        /// <response code="201">Offer created</response>
        /// <response code="401">Unauthorized request</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreateInquireResponse), 200)]
        public IActionResult CreateInquire([FromBody] CreateInquireRequest request,
            [FromServices] IValidator<CreateInquireRequest> validator,
            [FromServices] IApiKeyService apiKeyService) {

            ModelStateDictionary? errors = this.Validate<CreateInquireRequest>(validator, request);
            if (errors != null)
            {
                return ValidationProblem(errors);
            }

            return this.GetServiceIdFromHttpContext(apiKeyService).Match(serviceId => _inquireService.CreateInquire(request, serviceId).Result.Match(
                                response => CreatedAtAction(
                                    actionName: nameof(CreateInquire),
                                    routeValues: new { id = response.Code },
                                    value: response),
                                    errors => Problem(statusCode: errors.First.StatusCode, detail: errors.First.Message, title: errors.First.Title)),
                                errors => Problem(statusCode: errors.First.StatusCode, detail: errors.First.Message, title: errors.First.Title));
        }
    }

}
