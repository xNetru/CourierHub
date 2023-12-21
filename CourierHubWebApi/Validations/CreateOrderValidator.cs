using CourierHubWebApi.Models;
using FluentValidation;

namespace CourierHubWebApi.Validations
{
    public class CreateOrderValidator: AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidator() 
        {
            RuleFor(x => x.InquireCode);
            RuleFor(x => x.ClientName).Matches(@"^[A-Z][a-z]+").MaximumLength(50);
            RuleFor(x => x.ClientSurname).Matches(@"^[A-Z][a-z]+").MaximumLength(50);
            RuleFor(x => x.ClientEmail).Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").MaximumLength(50);
            RuleFor(x => x.ClientPhoneNumber).Matches(@"^[0-9]+").MaximumLength(12);
            RuleFor(x => x.ClientCompany).Matches(@"^[A-Za-z0-9]").MaximumLength(50); // possibly to be changed
            RuleFor(x => x.ClientCity).Matches(@"^[A-Z][a-z]+").MaximumLength(50);
            RuleFor(x => x.ClientPostalCode).Matches(@"^\d{2}-\d{3}$");
            RuleFor(x => x.ClientStreet).Matches(@"^[A-Z][a-z]+").MaximumLength(50);
            RuleFor(x => x.ClientAddressBuildingNumber).Matches(@"^[0-9]+[A-Za-z]").MaximumLength(6);
            RuleFor(x => x.ClientFlat).Matches(@"^[0-9]+[A-Za-z]").MaximumLength(6);
        }
    }
}
