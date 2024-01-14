using CourierHub.Api.Models.SzymoApi;
using FluentValidation;

namespace CourierHub.Validation {
    internal class SzymoDimensionsValidator : AbstractValidator<SzymoDimensions> {
        public SzymoDimensionsValidator() {
            RuleFor(x => x.dimensionUnit).Matches("^(Meters|Inches)$");
            RuleFor(x => x.height).GreaterThanOrEqualTo(0.2f).LessThanOrEqualTo(8.0f);
            RuleFor(x => x.width).GreaterThanOrEqualTo(0.2f).LessThanOrEqualTo(8.0f);
            RuleFor(x => x.length).GreaterThanOrEqualTo(0.2f).LessThanOrEqualTo(8.0f);
        }

    }
}
