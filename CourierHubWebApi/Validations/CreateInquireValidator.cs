using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using CourierHubWebApi.Models;
using ErrorOr;
using FluentValidation;
using System.Data.SqlTypes;

namespace CourierHubWebApi.Validations {
    public class CreateInquireRequestValidator : AbstractValidator<CreateInquireRequest> {
        private CourierHubDbContext _dbContext;
        public CreateInquireRequestValidator(CourierHubDbContext CourierHubDbContext) {
            _dbContext = CourierHubDbContext;

            // Client Id Validation
            // RuleFor(x => x.ClientId).Must(ClientId => BeNullOrExistInDatabaseAsync(ClientId).Result);

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
            RuleFor(x => x.SourceCity).Matches("[a-zA-Z]").MaximumLength(50);

            // Destination address validation 
            RuleFor(x => x.DestinationStreet).Matches("[a-zA-Z0-9]").MaximumLength(50);
            RuleFor(x => x.DestinationNumber).Matches("[a-zA-Z0-9]").MaximumLength(5);
            RuleFor(x => x.DestinationFlat).Matches("[a-zA-Z0-9]").MaximumLength(5);
            RuleFor(x => x.DestinationPostalCode).Matches("[0-9]").Length(5);
            RuleFor(x => x.DestinationCity).Matches("[a-zA-Z]").MaximumLength(50);

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
        private async Task<bool> BeNullOrExistInDatabaseAsync(int? ClientId)
        {
            if (ClientId == null)
                return true;
            User? result = await _dbContext.Users.FindAsync(ClientId);
            if (result == null)
                return false;
            if (result.Type == (int)UserType.Client)
                return true;
            return false;
        }
        private bool BeValidPriorityType(int Priority)
        {
            return Priority == (int)PriorityType.Low ||
                   Priority == (int)PriorityType.Medium ||
                   Priority == (int)PriorityType.High;
        }
    }
}
