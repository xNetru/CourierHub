using CourierHubWebApi.Models;
using FluentValidation;
using System.Data.SqlTypes;

namespace CourierHubWebApi.Validations {
    public class CreateInquireRequestValidator : AbstractValidator<CreateInquireRequest> {
        public CreateInquireRequestValidator() {
            // TODO: Client Id validation

            // Dimensions
            RuleFor(x => x.Depth).GreaterThan(0);
            RuleFor(x => x.Width).GreaterThan(0);
            RuleFor(x => x.Length).GreaterThan(0);
            RuleFor(x => x.Mass).GreaterThan(0);

            // TODO: Maximal values of dimensions

            // Source address validation
            RuleFor(x => x.SourceStreet).Matches("[a-zA-Z0-9]").MaximumLength(50);
            RuleFor(x => x.SourceNumber).Matches("[a-zA-Z0-9]").MaximumLength(5);
            RuleFor(x => x.SourceFlat).Matches("[a-zA-Z0-9]").MaximumLength(5);
            RuleFor(x => x.SourcePostalCode).Matches("[0-9]").Length(5);

            // Destination address validation 
            RuleFor(x => x.DestinationStreet).Matches("[a-zA-Z0-9]").MaximumLength(50);
            RuleFor(x => x.DestinationNumber).Matches("[a-zA-Z0-9]").MaximumLength(5);
            RuleFor(x => x.DestinationFlat).Matches("[a-zA-Z0-9]").MaximumLength(5);
            RuleFor(x => x.DestinationPostalCode).Matches("[0-9]").Length(5);

            // DateTime validation
            RuleFor(x => x.Datetime).Must(BeInSqlDateTimeRange);
            RuleFor(x => x.SourceDate).Must(BeInSqlDateTimeRange);
            RuleFor(x => x.DestinationDate).Must(BeInSqlDateTimeRange);
            RuleFor(x => x.Datetime).Must((x, DateTime) => DateTime <= x.SourceDate);
            RuleFor(x => x.SourceDate).Must((x, SourceDate) => SourceDate <= x.DestinationDate);

            // TODO: Priority Validation

        }
        private bool BeInSqlDateTimeRange(DateTime time) {
            return time >= (DateTime)SqlDateTime.MinValue &&
                   time <= (DateTime)SqlDateTime.MaxValue;
        }
    }
}
