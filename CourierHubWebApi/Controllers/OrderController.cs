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
            [FromServices] IValidator<CreateOrderRequest> validator) {
            ModelStateDictionary? errors = this.Validate<CreateOrderRequest>(validator, request);
            if (errors != null)
                return ValidationProblem(errors);

            return this.ExtractServiceIdFromContext().Match(
                serviceId => _orderService.CreateOrder(request, serviceId).Match(
                    statusCode => Ok(statusCode), errors => Problem(detail: errors.First().Description,
                    statusCode: errors.First().NumericType)),
                errors => Problem(detail: errors.First().Description, statusCode: errors.First().NumericType));
        }

    }
}
