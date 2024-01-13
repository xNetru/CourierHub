using CourierHub.Api.Models.SzymoApi;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Validation
{
    internal class SzymoAddressValidator: AbstractValidator<SzymoAddress>
    {
        public SzymoAddressValidator()
        {
            RuleFor(x => x.houseNumber).Matches(@"^([0-9]+[A-Za-z]*)");
            RuleFor(x => x.apartmentNumber).Matches(@"^([0-9]+[A-Za-z]*|[0-9]*)");
            RuleFor(x => x.street).Matches(@"^([A-ZŚŁŻŹĆŃ0-9][a-zęóąśłżźćń0-9]+)([-\s][A-ZŚŁŻŹĆŃ0-9][a-zęóąśłżźćń0-9]+)*");
            RuleFor(x => x.city).Matches(@"^([A-ZŚŁŻŹĆŃ][a-zęóąśłżźćń]+)([-\s][A-ZŚŁŻŹĆŃ][a-zęóąśłżźćń]+)*");
            RuleFor(x => x.zipCode).Matches(@"^\d{2}-\d{3}$");
            RuleFor(x => x.country).Matches(@"^([A-ZŚŁŻŹĆŃ][a-zęóąśłżźćń]+)([-\s][A-ZŚŁŻŹĆŃ][a-zęóąśłżźćń]+)*");
        }
    }
}
