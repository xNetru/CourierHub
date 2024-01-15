using CourierHubWebApi.Models;
using CourierHubWebApi.Validations;

namespace CourierHubWebApi.Test {
    public class ApiSideAddressValidatorTest {
        private ApiSideAddressValidator _validator = new();

        [Fact]
        public async Task Validate_ShouldRejectAddress_WhenGivenCityIsNotValid() {
            // Arrange
            ApiSideAddress address = new ApiSideAddress(
                City: "radogoszcz",
                PostalCode: "21-202",
                Street: "Spooner Street",
                Number: "20",
                Flat: null);
            // Act
            var result = await _validator.ValidateAsync(address);
            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Validate_ShouldRejectAddress_WhenGivenPostalCodeIsNotValid() {
            // Arrange
            ApiSideAddress address = new ApiSideAddress(
                City: "Radogoszcz",
                PostalCode: "21222u",
                Street: "Spooner Street",
                Number: "20",
                Flat: null);
            // Act
            var result = await _validator.ValidateAsync(address);
            // Assert
            Assert.False(result.IsValid);
        }


        [Fact]
        public async Task Validate_ShouldRejectAddress_WhenGivenStreetIsNotValid() {
            // Arrange
            ApiSideAddress address = new ApiSideAddress(
                City: "Radogoszcz",
                PostalCode: "21-202",
                Street: "Spooner+street",
                Number: "20",
                Flat: null);
            // Act
            var result = await _validator.ValidateAsync(address);
            // Assert
            Assert.False(result.IsValid);
        }


        [Fact]
        public async Task Validate_ShouldRejectAddress_WhenGivenHouseNumberIsNotValid() {
            // Arrange
            ApiSideAddress address = new ApiSideAddress(
                City: "Radogoszcz",
                PostalCode: "21-202",
                Street: "Spooner Street",
                Number: "a",
                Flat: null);
            // Act
            var result = await _validator.ValidateAsync(address);
            // Assert
            Assert.False(result.IsValid);
        }


        [Fact]
        public async Task Validate_ShouldRejectAddress_WhenGivenFlatNumberIsNotValid() {
            // Arrange
            ApiSideAddress address = new ApiSideAddress(
                City: "Radogoszcz",
                PostalCode: "21-202",
                Street: "Spooner Street",
                Number: "20",
                Flat: "abc");
            // Act
            var result = await _validator.ValidateAsync(address);
            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Validate_ShouldAcceptAddress_WhenGivenAddressIsValid() {
            // Arrange
            ApiSideAddress address = new ApiSideAddress(
                City: "Biała-Podlaska",
                PostalCode: "07-293",
                Street: "Żyrardowska",
                Number: "21a",
                Flat: null);
            // Act
            var result = await _validator.ValidateAsync(address);
            // Assert
            Assert.True(result.IsValid);
        }

    }
}
