using CourierHubWebApi.Models;
using CourierHubWebApi.Services.Contracts;
using FluentValidation;

namespace CourierHubWebApi.Validations
{
    public class CreateOrderValidator: AbstractValidator<CreateOrderRequest>
    {
        private ApiSideAddressValidator _apiSideAddressValidator;
        private IPriceCacheService _priceCacheService;
        
        public CreateOrderValidator(ApiSideAddressValidator addressValidator, IPriceCacheService priceCacheService) 
        {
            _apiSideAddressValidator = addressValidator; 
            _priceCacheService = priceCacheService;
            RuleFor(x => x.InquireCode).MaximumLength(50);
            RuleFor(x => x.ClientName).Matches(@"^[A-Z][a-z]+").MaximumLength(50);
            RuleFor(x => x.ClientSurname).Matches(@"^[A-Z][a-z]+").MaximumLength(50);
            RuleFor(x => x.ClientEmail).Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").MaximumLength(50);
            RuleFor(x => x.ClientPhoneNumber).Matches(@"^[0-9]+").MaximumLength(12);
            RuleFor(x => x.ClientCompany).Matches(@"^[A-Za-z0-9]").MaximumLength(50); // possibly to be changed
            RuleFor(x => x.ClientAddress).SetValidator(addressValidator);
        }
    }
}
