using CourierHubWebApi.Models;
using CourierHubWebApi.Validations;

namespace CourierHubWebApi.Test {
    public class CreateInquireRequestValidatorTest {
        private CreateInquireRequestValidator _validator = new();

        [Fact]
        public async Task Validate_ShouldRejectRequest_WhenWrongAddressIsGiven() {
            ApiSideAddress address;
        }
    }
}
