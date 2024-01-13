using CourierHub.Shared.SzymoApiModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Shared.Validation
{
    public class SzymoInquiryValidator: AbstractValidator<SzymoInquiry>
    {
        public SzymoInquiryValidator()
        {
            RuleFor(x => x.weight).GreaterThanOrEqualTo(0.2f).LessThanOrEqualTo(8.0f);
            RuleFor(x => x.weightUnit).Matches("^(Kilograms|Pounds)$");
            RuleFor(x => x.currency).Matches("^(Usd|Pln|Eur|Gbp)$");
            RuleFor(x => x.priority).Matches("^(Low|Medium|High)$");
            RuleFor(x => x.dimensions).SetValidator(new SzymoDimensionsValidator());

            var addressValidator = new SzymoAddressValidator();
            RuleFor(x => x.source).SetValidator(addressValidator);
            RuleFor(x => x.destination).SetValidator(addressValidator);
            RuleFor(x => x.pickupDate).Must((x, pickupDate) => x.deliveryDay >= pickupDate);
        }
    }
}
