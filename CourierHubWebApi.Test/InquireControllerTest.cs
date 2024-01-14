using CourierHub.Shared.Data;
using CourierHubWebApi.Controllers;
using Moq;

namespace CourierHubWebApi.Test {
    internal class InquireControllerTest {
        Mock<CourierHubDbContext> _mockContext;
        InquireController _controller;

        public InquireControllerTest() {
            _mockContext = new Mock<CourierHubDbContext>();
        }
    }
}
