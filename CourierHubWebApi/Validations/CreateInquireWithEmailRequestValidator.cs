using CourierHub.Shared.Data;
using CourierHubWebApi.Models;
using FluentValidation;

namespace CourierHubWebApi.Validations {
    public class CreateInquireWithEmailRequestValidator : AbstractValidator<CreateInquireWithEmailRequest> {
        private CreateInquireRequestValidator _standardRequestValidator;
        public CreateInquireWithEmailRequestValidator(CourierHubDbContext dbContext) {
            _standardRequestValidator = new CreateInquireRequestValidator(dbContext);
            RuleFor(x => x.standardRequest).SetValidator(_standardRequestValidator);
            RuleFor(x => x.Email).Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").MaximumLength(50);
        }
    }
}
