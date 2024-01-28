using CourierHub.Cloud;
using CourierHub.Shared.Enums;
using CourierHubWebApi.Examples;
using CourierHubWebApi.Extensions;
using CourierHubWebApi.Models;
using CourierHubWebApi.Services.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics;

namespace CourierHubWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase {
        private IOrderService _orderService;
        public OrderController(IOrderService orderService) {
            _orderService = orderService;
        }

        /// <summary>
        /// Creates order based on passed offer
        /// </summary>
        /// <param name="request">Order data</param>
        /// <param name="validator">Validator</param>
        /// <param name="apiKeyService">ApiKeyService</param>
        /// <param name="logger">Logger</param>
        /// <returns>Returns status code indicating whether the order creation succeded</returns>
        /// <response code="200">Order created successfully</response>
        /// <response code="401">Unauthorized request</response>
        /// <response code="404">No such offer exists</response>
        /// <response code="408">Offer expired</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        public IActionResult CreateOrder([FromBody] CreateOrderRequest request,
            [FromServices] IValidator<CreateOrderRequest> validator,
            [FromServices] IApiKeyService apiKeyService) {

            ModelStateDictionary? errors = this.Validate<CreateOrderRequest>(validator, request);
            if (errors != null)
            {
                return ValidationProblem(errors);
            }

            return this.GetServiceIdFromHttpContext(apiKeyService).Match(
                serviceId => {
                    return _orderService.CreateOrder(request, serviceId).Match(
                    statusCode => Ok(statusCode),
                    errors => Problem(statusCode: errors.First.StatusCode, detail: errors.First.Message, title: errors.First.Title));
                },
                errors => Problem(statusCode: errors.First.StatusCode, detail: errors.First.Message, title: errors.First.Title));
        }
        
        /// <summary>
        /// Returns order status code
        /// </summary>
        /// <param name="code">Order code</param>
        /// <param name="validator">Validator</param>
        /// <param name="apiKeyService">ApiKeyService</param>
        /// <param name="logger">Logger</param>
        /// <returns>Returns order status as integer</returns>
        /// <response code="200">Order status successfully</response>
        /// <response code="401">Unauthorized request</response>
        /// <response code="404">No such order exists</response>
        /// <response code="408">Order cancellation time elapsed</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("Status/{code}")]
        [ProducesResponseType(typeof(StatusType), StatusCodes.Status200OK)]
        public IActionResult GetOrderStatus(string code,
            [FromServices] IValidator<GetOrderStatusRequest> validator,
            [FromServices] IApiKeyService apiKeyService)
        {

            GetOrderStatusRequest request = new(code);
            ModelStateDictionary? errors = this.Validate<GetOrderStatusRequest>(validator, request);
            if (errors != null)
            {
                return ValidationProblem(errors);
            }

            return this.GetServiceIdFromHttpContext(apiKeyService).Match(
                serviceId => _orderService.GetOrderStatus(request, serviceId).Match(
                    orderStatusCode => Ok(orderStatusCode), 
                    errors => Problem(statusCode: errors.First.StatusCode, detail: errors.First.Message, title: errors.First.Title)),
                errors => Problem(statusCode: errors.First.StatusCode, detail: errors.First.Message, title: errors.First.Title));
        }

        /// <summary>
        /// Withdraws order
        /// </summary>
        /// <param name="request">Order code</param>
        /// <param name="validator">Validator</param>
        /// <param name="apiKeyService">ApiKeyService</param>
        /// <param name="logger">Logger</param>
        /// <returns>Returns status code indicating whether the order creation succeded</returns>
        /// <response code="200">Order withdrawn successfully</response>
        /// <response code="401">Unauthorized request</response>
        /// <response code="404">No such order exists</response>
        /// <response code="408">Order cancellation time elapsed</response>
        /// <response code="500">Internal server error</response>
        [HttpPut("Withdraw")]
        public IActionResult WithdrawOrder([FromBody] WithdrawOrderRequest request,
            [FromServices] IValidator<WithdrawOrderRequest> validator,
            [FromServices] IApiKeyService apiKeyService)
        {
            ModelStateDictionary? errors = this.Validate<WithdrawOrderRequest>(validator, request);
            if (errors != null)
            {
                return ValidationProblem(errors);
            }

            return this.GetServiceIdFromHttpContext(apiKeyService).Match(
                serviceId => _orderService.WithdrawOrder(request, serviceId).Result.Match(
                    statusCode => Ok(statusCode),
                    errors => Problem(statusCode: errors.First.StatusCode, detail: errors.First.Message, title: errors.First.Title)),
                errors => Problem(statusCode: errors.First.StatusCode, detail: errors.First.Message, title: errors.First.Title));
        }
    }
}
