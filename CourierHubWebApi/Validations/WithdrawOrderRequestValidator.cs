using CourierHubWebApi.Models;
using FluentValidation;

namespace CourierHubWebApi.Validations {
    public class WithdrawOrderRequestValidator : AbstractValidator<WithdrawOrderRequest> {
        OrderCodeValidator _orderCodeValidator = new OrderCodeValidator();
        public WithdrawOrderRequestValidator() {
            RuleFor(x => x.Code).SetValidator(_orderCodeValidator);
        }
    }
}
