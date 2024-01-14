using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourierHubWebApi.Services;
using CourierHub.Shared.Models;
using CourierHub.Shared.Data;
using Moq;
using Moq.EntityFrameworkCore;

namespace CourierHubWebApi.Test
{
    public class ApiKeyServiceTest
    {
        private ApiKeyService _apiKeyService;
        private Mock<CourierHubDbContext> _mockContext;
        public ApiKeyServiceTest()
        {
            _mockContext = new Mock<CourierHubDbContext>();
            IList<Service> services = new List<Service>(){
                new Service() {Id = 1, Name = "SusCourierHub", ApiKey = "a1"},
                new Service() {Id = 2, Name = "StachuHub", ApiKey = "b2"}
            };
            _mockContext.Setup(x => x.Services).ReturnsDbSet(services);

            _apiKeyService = new ApiKeyService(_mockContext.Object);
        }
        [Fact]
        public void TryGetServiceId_ShouldReturnFalse_WhenWrongApiKeyIsGiven()
        {
            // Arrange
            string apikKey = "Bydgoszcz";
            int serviceId = -1;
            // Act
            bool result = _apiKeyService.TryGetServiceId(apikKey, out serviceId);
            // Assert
            Assert.False(result);
            Assert.Equal(-1, serviceId);
        }

        [Fact]
        public void TryGetServiceId_ShouldReturnTrueAndServiceId_WhenCorrectApiKeyIsGiven()
        {
            // Arrange
            string apiKey = "a1";
            int serviceId = -1; 
            // Act
            bool result = _apiKeyService.TryGetServiceId(apiKey, out serviceId);
            // Assert
            Assert.True(result);
            Assert.Equal(1, serviceId);
        }

        // public void IsOurServiceRequest_ShouldReturnFalse_When

    }
}
