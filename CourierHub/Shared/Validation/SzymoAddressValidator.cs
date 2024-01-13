using CourierHub.Shared.SzymoApiModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.Validation
{
    internal class SzymoAddressValidator: AbstractValidator<SzymoAddress>
    {
        public SzymoAddressValidator()
        {
            RuleFor(x => x.houseNumber).Matches(@"^[0-9]+[A-Za-z]*");
            RuleFor(x => x.apartmentNumber).Matches(@"^[0-9]+[A-Za-z]*");
            RuleFor(x => x.street).Matches(@"^[A-Z][a-z]+");
            RuleFor(x => x.city).Matches(@"^[A-Z][a-z]+");
            RuleFor(x => x.zipCode).Matches(@"^\d{2}-\d{3}$");
            RuleFor(x => x.country).Matches(@"^[A-Z][a-z]+");
        }
    }
}
