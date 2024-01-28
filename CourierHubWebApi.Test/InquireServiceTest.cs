using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using CourierHubWebApi.Errors;
using CourierHubWebApi.Models;
using CourierHubWebApi.Services;
using Moq;
using Moq.EntityFrameworkCore;
using OneOf;

namespace CourierHubWebApi.Test {
    public class InquireServiceTest {
        private InquireService _inquireService;
        private Mock<CourierHubDbContext> _mockContext;

        public InquireServiceTest() {
            _mockContext = new Mock<CourierHubDbContext>();

            IList<Inquire> inquires = new List<Inquire>();
            _mockContext.Setup(x => x.Inquires).ReturnsDbSet(inquires);

            IList<Service> services = new List<Service>()
            {
                new Service(){Id = 1, Name = "CourierHub", ApiKey = "susKey"},
                new Service(){Id = 2, Name = "PedroHub", ApiKey = "pedroKey"}
            };
            _mockContext.Setup(x => x.Services).ReturnsDbSet(services);

            IList<Scaler> scalers = new List<Scaler>()
            {
                new Scaler()
                {
                    Name = "Test",
                    Id = 0,
                    Length = 1m,
                    Depth = 1m,
                    Width = 1m,
                    Fee = 1m,
                    Distance = 1m,
                    Tax = 1.0f,
                    Company = 1m,
                    Mass = 1m,
                    Priority = 1m,
                    Time = 1m,
                    Weekend = 1m
                }
            };
            _mockContext.Setup(x => x.Scalers).ReturnsDbSet(scalers);

            _inquireService = new InquireService(_mockContext.Object,
                                                new PriceCacheService(),
                                                new ApiKeyService(_mockContext.Object));

        }

        [Fact]
        public async Task CreateInquire_ShouldCreateOffer_WhenValidInquiryIsGiven() {
            // Arrange
            ApiSideAddress source = new ApiSideAddress(
                City: "New York",
                PostalCode: "67-298",
                Street: "Forster Avenue",
                Number: "23",
                Flat: null);

            ApiSideAddress destination = new ApiSideAddress(
                City: "Trenton",
                PostalCode: "56-298",
                Street: "Edgewood Road",
                Number: "4",
                Flat: "12");

            DateTime requestTime = DateTime.Now;
            DateTime sourceTime = requestTime.AddMonths(1).AddDays(10);
            DateTime destinationTime = requestTime.AddMonths(1).AddDays(20);

            CreateInquireRequest inquiry = new CreateInquireRequest(
                Depth: 890,
                Width: 989,
                Length: 193,
                Mass: 1000,
                SourceAddress: source,
                DestinationAddress: destination,
                SourceDate: sourceTime,
                DestinationDate: destinationTime,
                Datetime: requestTime,
                IsCompany: true,
                IsWeekend: destinationTime.DayOfWeek == DayOfWeek.Saturday || destinationTime.DayOfWeek == DayOfWeek.Sunday,
                PriorityType.Low);
            // Act
            OneOf<CreateInquireResponse, ApiError> result = await _inquireService.CreateInquire(inquiry, 1);
            string invalidCode = "Falenizza";
            CreateInquireResponse response = result.Match(x => x, x => new CreateInquireResponse(Price: 0m, Code: invalidCode, ExpirationDate: DateTime.Now));
            // Assert
            Assert.NotEqual(invalidCode, response.Code);
        }
    }
}
