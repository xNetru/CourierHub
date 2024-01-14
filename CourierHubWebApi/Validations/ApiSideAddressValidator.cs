using CourierHubWebApi.Models;
using FluentValidation;

namespace CourierHubWebApi.Validations {
    public class ApiSideAddressValidator : AbstractValidator<ApiSideAddress> {
        public ApiSideAddressValidator() {
            RuleFor(x => x.City).Matches(@"^([A-ZŚŁŻŹĆŃ][a-zęóąśłżźćń]+)([-\s][A-ZŚŁŻŹĆŃ][a-zęóąśłżźćń]+)*$").MaximumLength(50);
            RuleFor(x => x.PostalCode).Matches(@"^\d{2}-\d{3}$");
            RuleFor(x => x.Street).Matches(@"^([A-ZŚŁŻŹĆŃ0-9][a-zęóąśłżźćń0-9]+)([-\s][A-ZŚŁŻŹĆŃ0-9][a-zęóąśłżźćń0-9]+)*$").MaximumLength(50);
            RuleFor(x => x.Number).Matches(@"^([0-9]+[A-Za-z]*)$").MaximumLength(6);
            RuleFor(x => x.Flat).Matches(@"^([0-9]+[A-Za-z]*|[0-9]*)$").MaximumLength(6);
        }
    }
}
