using CourierHubWebApi.Services;

namespace CourierHubWebApi.Test {
    public class PriceCacheServiceTest {
        PriceCacheService _emptyPriceCacheService = new PriceCacheService();
        PriceCacheService _filledPriceCacheService;
        public PriceCacheServiceTest() {
            _filledPriceCacheService = new PriceCacheService();
            _filledPriceCacheService.SavePrice("First", 20.12m);
            _filledPriceCacheService.SavePrice("Second", 29.99m);
        }

        [Fact]
        public void SavePrice_ShouldReturnTimestamp_WhenNewInquiryIsPassed() {
            // Arrange 
            string inquiryCode = "Third";
            decimal price = 45.99m;
            // Act
            bool result = _filledPriceCacheService.SavePrice(inquiryCode, price).Match(x => true, x => false);
            // Assert
            Assert.True(result);
        }
        [Fact]
        public void SavePrice_ShouldReturnError_WhenExistingInquiryIsPassed() {
            // Arrange 
            string inquiryCode = "First";
            decimal price = 23.99m;
            // Act 
            bool result = _filledPriceCacheService.SavePrice(inquiryCode, price).Match(x => false, x => true);
            // Assert
            Assert.True(result);
        }
        [Fact]
        public void SavePrice_ShouldReturnFutureTimeStamp_WhenNewInquiryIsPassed() {
            // Arrange
            string inquiryCode = "Fourth";
            decimal price = 43.56m;
            // Act
            DateTime result = _filledPriceCacheService.SavePrice(inquiryCode, price).Match(x => x, x => DateTime.MinValue);
            // Assert
            Assert.True(result > DateTime.Now);
        }
        [Fact]
        public void GetPrice_ShouldReturnPrice_WhenValidObtainmentTimeIsGiven() {
            // Arrange
            string inquiryCode = "Fifth";
            decimal price = 89.99m;
            DateTime result = _filledPriceCacheService.SavePrice(inquiryCode, price).Match(x => x, x => DateTime.MaxValue);
            DateTime obtainmentTime = result.AddMinutes(-5);
            // Act
            decimal returnedPrice = _filledPriceCacheService.GetPrice(inquiryCode, obtainmentTime).Match(x => x, x => -1.0m);
            // Assert
            Assert.Equal(price, returnedPrice);
        }
    }
}
