using CourierHubWebApi.Models;
using FluentValidation;

namespace CourierHubWebApi.Validations {
    public class GetOrderStatusRequestValidator : AbstractValidator<GetOrderStatusRequest> {
        private OrderCodeValidator _orderCodeValidator = new OrderCodeValidator();
        public GetOrderStatusRequestValidator() {
            RuleFor(x => x.Code).SetValidator(_orderCodeValidator);
        }
    }
}
