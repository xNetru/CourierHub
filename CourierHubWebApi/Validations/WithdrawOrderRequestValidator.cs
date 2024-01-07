using FluentValidation;
using CourierHubWebApi.Models;

namespace CourierHubWebApi.Validations
{
    public class WithdrawOrderRequestValidator: AbstractValidator<WithdrawOrderRequest>
    {
        OrderCodeValidator _orderCodeValidator = new OrderCodeValidator();
        public WithdrawOrderRequestValidator()
        {
            RuleFor(x => x.Code).SetValidator(_orderCodeValidator);
        }
    }
}
