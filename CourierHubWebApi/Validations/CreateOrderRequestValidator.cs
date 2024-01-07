using CourierHub.Shared.Data;
using CourierHubWebApi.Models;
using FluentValidation;

namespace CourierHubWebApi.Validations {
    public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest> {
        //private ApiSideAddressValidator _apiSideAddressValidator;
        private CourierHubDbContext _dbContext;
        private static ApiSideAddressValidator _apiSideAddressValidator = new ApiSideAddressValidator();


        public CreateOrderRequestValidator(CourierHubDbContext dbContext) {
            _dbContext = dbContext;

            RuleFor(x => x.InquireCode).Must(BeValidInquireCode).MaximumLength(50);
            RuleFor(x => x.InquireCode).MaximumLength(50);
            RuleFor(x => x.ClientName).Matches(@"^[A-Z][a-z]+").MaximumLength(50);
            RuleFor(x => x.ClientSurname).Matches(@"^[A-Z][a-z]+").MaximumLength(50);
            RuleFor(x => x.ClientEmail).Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").MaximumLength(50);
            RuleFor(x => x.ClientPhoneNumber).Matches(@"^[0-9]+").MaximumLength(12);
            RuleFor(x => x.ClientCompany).Matches(@"^[A-Za-z0-9]").MaximumLength(50); // possibly to be changed
            RuleFor(x => x.ClientAddress).SetValidator(_apiSideAddressValidator);
        }
        public bool BeValidInquireCode(string code) {
            return _dbContext.Inquires.Select(x => x.Code).Where(x => x == code).Count() == 1;
        }

    }
}
