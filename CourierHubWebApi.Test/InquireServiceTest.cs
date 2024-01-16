using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using CourierHubWebApi.Services;
using Moq;
using Moq.EntityFrameworkCore;
using CourierHubWebApi.Models;
using MaxMind.GeoIP2.Model;
using CourierHub.Shared.Enums;
using OneOf;
using CourierHubWebApi.Errors;

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

            _inquireService = new InquireService(_mockContext.Object,
                                                new PriceCacheService(),
                                                new ApiKeyService(_mockContext.Object));

        }

        [Fact]
        public void CreateInquire_ShouldCreateOffer_WhenValidInquiryIsGiven() {
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
            DateTime destincationTime = requestTime.AddMonths(1).AddDays(20);

            CreateInquireRequest inquiry = new CreateInquireRequest(
                Depth: 890,
                Width: 989,
                Length: 193,
                Mass: 1000,
                SourceAddress: source,
                DestinationAddress: destination,
                SourceDate: sourceTime,
                DestinationDate: destincationTime,
                Datetime: requestTime,
                IsCompany: true,
                IsWeekend: destincationTime.DayOfWeek == DayOfWeek.Saturday || destincationTime.DayOfWeek == DayOfWeek.Sunday,
                PriorityType.Low);
            // Act
            OneOf<CreateInquireResponse, ApiError> result = _inquireService.CreateInquire(inquiry, 1).Result;
            CreateInquireResponse? response = result.Match(x => x, x => null);
            // Assert
            Assert.NotNull(response);
        }
    }
}
