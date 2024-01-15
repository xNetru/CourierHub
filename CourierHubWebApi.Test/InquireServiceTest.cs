using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using CourierHubWebApi.Services;
using Moq;
using Moq.EntityFrameworkCore;

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

            // Act
            // Assert
        }
    }
}
