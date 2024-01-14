using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using CourierHubWebApi.Models;
using FluentValidation;
using System.Data.SqlTypes;

namespace CourierHubWebApi.Validations {
    public class CreateInquireRequestValidator : AbstractValidator<CreateInquireRequest> {
        private static ApiSideAddressValidator _apiSideAddressValidator = new ApiSideAddressValidator();

        public CreateInquireRequestValidator() {
            // Dimensions
            RuleFor(x => x.Depth).GreaterThan(0);
            RuleFor(x => x.Width).GreaterThan(0);
            RuleFor(x => x.Length).GreaterThan(0);
            RuleFor(x => x.Mass).GreaterThan(0);

            // TODO: Maximal values of dimensions

            // Source address validation
            RuleFor(x => x.SourceAddress).SetValidator(_apiSideAddressValidator);

            // Destination address validation 
            RuleFor(x => x.DestinationAddress).SetValidator(_apiSideAddressValidator);

            // DateTime validation
            RuleFor(x => x.Datetime).Must(BeInSqlDateTimeRange);
            RuleFor(x => x.SourceDate).Must(BeInSqlDateTimeRange);
            RuleFor(x => x.DestinationDate).Must(BeInSqlDateTimeRange);
            RuleFor(x => x.Datetime).Must((x, DateTime) => DateTime <= x.SourceDate);
            RuleFor(x => x.SourceDate).Must((x, SourceDate) => SourceDate <= x.DestinationDate);

            // Priority Validation
            RuleFor(x => x.Priority).Must(BeValidPriorityType);
        }
        private bool BeInSqlDateTimeRange(DateTime time) {
            return time >= (DateTime)SqlDateTime.MinValue &&
                   time <= (DateTime)SqlDateTime.MaxValue;
        }
        private bool BeValidPriorityType(int Priority) {
            return Priority == (int)PriorityType.Low ||
                   Priority == (int)PriorityType.Medium ||
                   Priority == (int)PriorityType.High;
        }
    }
}
