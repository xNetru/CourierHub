using CourierHub.Shared.Enums;
using CourierHubWebApi.Models;
using CourierHubWebApi.Validations;

namespace CourierHubWebApi.Test {
    public class CreateInquireRequestValidatorTest {
        private CreateInquireRequestValidator _validator = new();

        [Fact]
        public async Task Validate_ShouldRejectRequest_WhenWrongAddressIsGiven() {
            // Arrange
            ApiSideAddress source = new ApiSideAddress(
                City: "Lipinki Łużyckie",
                PostalCode: "02-1202",
                Street: "bydgoska",
                Number: "23",
                Flat: "2");

            ApiSideAddress destination = new ApiSideAddress(
                City: "Bielsko-Biała",
                PostalCode: "13-203",
                Street: "Krakowska",
                Number: "65",
                Flat: "31a");

            CreateInquireRequest request = new CreateInquireRequest(
                Depth: 1,
                Width: 2,
                Length: 4,
                Mass: 200,
                SourceAddress: source,
                DestinationAddress: destination,
                SourceDate: new DateTime(2025, 10, 21),
                DestinationDate: new DateTime(2025, 11, 1),
                Datetime: new DateTime(2025, 9, 10),
                IsCompany: true,
                IsWeekend: false,
                Priority: PriorityType.High);
            // Act
            var result = await _validator.ValidateAsync(request);
            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Validate_ShouldRejectRequest_WhenWrongDimensionsAreGiven() {
            // Arrange
            ApiSideAddress source = new ApiSideAddress(
                City: "Lipinki Łużyckie",
                PostalCode: "02-120",
                Street: "Bydgoska",
                Number: "23",
                Flat: "2");

            ApiSideAddress destination = new ApiSideAddress(
                City: "Bielsko-Biała",
                PostalCode: "13-203",
                Street: "Krakowska",
                Number: "65",
                Flat: "31a");

            CreateInquireRequest request = new CreateInquireRequest(
                Depth: -1,
                Width: 2,
                Length: 4,
                Mass: 0,
                SourceAddress: source,
                DestinationAddress: destination,
                SourceDate: new DateTime(2025, 10, 21),
                DestinationDate: new DateTime(2025, 11, 1),
                Datetime: new DateTime(2025, 9, 10),
                IsCompany: true,
                IsWeekend: false,
                Priority: PriorityType.High);
            // Act
            var result = await _validator.ValidateAsync(request);
            // Assert
            Assert.False(result.IsValid);
        }


        [Fact]
        public async Task Validate_ShouldRejectRequest_WhenWrongTimeIsGiven() {
            // Arrange
            ApiSideAddress source = new ApiSideAddress(
                City: "Lipinki Łużyckie",
                PostalCode: "02-120",
                Street: "bydgoska",
                Number: "23",
                Flat: "2");

            ApiSideAddress destination = new ApiSideAddress(
                City: "Bielsko-Biała",
                PostalCode: "13-203",
                Street: "Krakowska",
                Number: "65",
                Flat: "31a");

            CreateInquireRequest request = new CreateInquireRequest(
                Depth: 1,
                Width: 2,
                Length: 4,
                Mass: 220,
                SourceAddress: source,
                DestinationAddress: destination,
                SourceDate: new DateTime(2025, 10, 21),
                DestinationDate: new DateTime(2023, 11, 1),
                Datetime: new DateTime(2025, 9, 10),
                IsCompany: true,
                IsWeekend: false,
                Priority: PriorityType.High);
            // Act
            var result = await _validator.ValidateAsync(request);
            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Validate_ShouldRejectRequest_WhenWrongPriorityIsGiven() {
            // Arrange
            ApiSideAddress source = new ApiSideAddress(
                City: "Lipinki Łużyckie",
                PostalCode: "02-120",
                Street: "Bydgoska",
                Number: "23",
                Flat: "2");

            ApiSideAddress destination = new ApiSideAddress(
                City: "Bielsko-Biała",
                PostalCode: "13-203",
                Street: "Krakowska",
                Number: "65",
                Flat: "31a");

            CreateInquireRequest request = new CreateInquireRequest(
                Depth: 1,
                Width: 2,
                Length: 4,
                Mass: 220,
                SourceAddress: source,
                DestinationAddress: destination,
                SourceDate: new DateTime(2025, 10, 21),
                DestinationDate: new DateTime(2025, 11, 1),
                Datetime: new DateTime(2025, 9, 10),
                IsCompany: true,
                IsWeekend: false,
                Priority: (PriorityType)10);
            // Act
            var result = await _validator.ValidateAsync(request);
            // Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Validate_ShouldAcceptRequest_WhenValidInquiryIsGiven() {
            // Arrange
            ApiSideAddress source = new ApiSideAddress(
                City: "Ligota Łabęcka",
                PostalCode: "02-120",
                Street: "Sośnicowicka",
                Number: "20a",
                Flat: "7");

            ApiSideAddress destination = new ApiSideAddress(
                City: "Kędzierzyn-Koźle",
                PostalCode: "20-391",
                Street: "Śląska",
                Number: "7",
                Flat: "128f");

            CreateInquireRequest request = new CreateInquireRequest(
                Depth: 1011,
                Width: 2232,
                Length: 4124,
                Mass: 220,
                SourceAddress: source,
                DestinationAddress: destination,
                SourceDate: new DateTime(2025, 10, 21),
                DestinationDate: new DateTime(2025, 11, 1),
                Datetime: new DateTime(2025, 9, 10),
                IsCompany: true,
                IsWeekend: false,
                Priority: PriorityType.High);
            // Act
            var result = await _validator.ValidateAsync(request);
            // Assert
            Assert.True(result.IsValid);
        }
    }
}
