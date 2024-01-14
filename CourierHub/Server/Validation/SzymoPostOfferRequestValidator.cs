using CourierHub.Api.Models.SzymoApi;
using FluentValidation;

namespace CourierHub.Validation {
    public class SzymoPostOfferRequestValidator : AbstractValidator<SzymoPostOfferRequest> {
        public SzymoPostOfferRequestValidator() {
            var addressValidator = new SzymoAddressValidator();
            RuleFor(x => x.address).SetValidator(addressValidator);
            RuleFor(x => x.email).Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            RuleFor(x => x.inquiryId);
        }
    }
}
