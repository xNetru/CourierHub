using CourierHubWebApi.Extensions;
using CourierHubWebApi.Models;
using CourierHubWebApi.Services.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CourierHubWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase {
        private IOrderService _orderService;
        public OrderController(IOrderService orderService) {
            _orderService = orderService;
        }
        [HttpPost]
        public IActionResult CreateOrder(CreateOrderRequest request,
            [FromServices] IValidator<CreateOrderRequest> validator,
            [FromServices] IApiKeyService apiKeyService) {
            ModelStateDictionary? errors = this.Validate<CreateOrderRequest>(validator, request);
            if (errors != null)
                return ValidationProblem(errors);

            return this.GetServiceIdFromHttpContext(apiKeyService).Match(
                serviceId => {
                    return _orderService.CreateOrder(request, serviceId).Match(
                    statusCode => Ok(statusCode),
                    errors => Problem(statusCode: errors.First.StatusCode, detail: errors.First.Message, title: errors.First.Title));
                },
                errors => Problem(detail: errors.First().Description, statusCode: errors.First().NumericType));
        }
        [HttpPut("Withdraw")]
        public IActionResult WithdrawOrder(WithdrawOrderRequest request,
            [FromServices] IValidator<WithdrawOrderRequest> validator,
            [FromServices] IApiKeyService apiKeyService) {
            ModelStateDictionary? errors = this.Validate<WithdrawOrderRequest>(validator, request);
            if (errors != null)
                return ValidationProblem(errors);

            return this.GetServiceIdFromHttpContext(apiKeyService).Match(
                serviceId => _orderService.WithdrawOrder(request, serviceId).Result.Match(
                    statusCode => Ok(statusCode), 
                    errors => Problem(statusCode: errors.First.StatusCode, detail: errors.First.Message, title: errors.First.Title)),
                errors => Problem(detail: errors.First().Description, statusCode: errors.First().NumericType));
        }

        [HttpGet("Status/{code}")]
        public IActionResult GetOrderStatus(string code,
            [FromServices] IValidator<GetOrderStatusRequest> validator,
            [FromServices] IApiKeyService apiKeyService) {
            GetOrderStatusRequest request = new(code);
            ModelStateDictionary? errors = this.Validate<GetOrderStatusRequest>(validator, request);
            if (errors != null)
                return ValidationProblem(errors);

            return this.GetServiceIdFromHttpContext(apiKeyService).Match(
                serviceId => _orderService.GetOrderStatus(request, serviceId).Match(
                    orderStatusCode => Ok(orderStatusCode), errors => Problem(statusCode: errors.First.StatusCode, detail: errors.First.Message, title: errors.First.Title)),
                errors => Problem(detail: errors.First().Description, statusCode: errors.First().NumericType));
        }
    }
}
