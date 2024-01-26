using CourierHub.Shared.Logging.Contracts;
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
        /// <response code="200">Offer created</response>
        /// <response code="401">Unauthorized request</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CreateInquireResponse), 200)]
        public IActionResult CreateInquire([FromBody] CreateInquireRequest request,
            [FromServices] IValidator<CreateInquireRequest> validator,
            [FromServices] IApiKeyService apiKeyService,
            [FromServices] IMyLogger logger) {
            // time measurement start
            Stopwatch stopwatch = Stopwatch.StartNew();

            PrepareBlobPathAndContainer(logger);
            logger.blobData.BlobBuilder.AddRequest(request);

            ModelStateDictionary? errors = this.Validate<CreateInquireRequest>(validator, request);
            if (errors != null) {
                logger.blobData.BlobBuilder.AddError(errors);
                logger.blobData.BlobBuilder.AddStatusCode(StatusCodes.Status400BadRequest);
                logger.blobData.BlobBuilder.AddOperationTime(stopwatch.Elapsed);
                return ValidationProblem(errors);
            }

            ObjectResult result = this.GetServiceIdFromHttpContext(apiKeyService).Match(serviceId => _inquireService.CreateInquire(request, serviceId).Result.Match(
                                response => CreatedAtAction(
                                    actionName: nameof(CreateInquire),
                                    routeValues: new { id = response.Code },
                                    value: response),
                                    errors => Problem(statusCode: errors.First.StatusCode, detail: errors.First.Message, title: errors.First.Title)),
                                errors => Problem(statusCode: errors.First.StatusCode, detail: errors.First.Message, title: errors.First.Title));

            int? statusCode = result.StatusCode;
            if (statusCode != null)
                logger.blobData.BlobBuilder.AddStatusCode((int)statusCode);
            logger.blobData.BlobBuilder.AddResponse(result);
            logger.blobData.BlobBuilder.AddOperationTime(stopwatch.Elapsed);
            if (!logger.SaveLog().Result)
                Console.WriteLine("Could not save log");
            return result;
        }
        private void PrepareBlobPathAndContainer(IMyLogger logger) {
            DateTime now = DateTime.Now;
            logger.blobData.PathBuilder.AddApplication(Applications.API);
            logger.blobData.PathBuilder.AddDate(DateOnly.FromDateTime(now));
            logger.blobData.PathBuilder.AddController("InquireController");
            logger.blobData.PathBuilder.AddMethod("POST");
            logger.blobData.PathBuilder.AddTime(now.TimeOfDay);

            logger.blobData.ContainerBuilder.AddLogs();
        }
    }

}
