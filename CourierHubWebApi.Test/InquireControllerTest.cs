using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using CourierHubWebApi.Controllers;
using Moq;
using Moq.EntityFrameworkCore;

namespace CourierHubWebApi.Test {
    internal class InquireControllerTest {
        Mock<CourierHubDbContext> _mockContext;
        InquireController _controller;

        public InquireControllerTest() {
            _mockContext = new Mock<CourierHubDbContext>();
            IList<Inquire> inquires = new List<Inquire>();
            _mockContext.Setup(x => x.Inquires).ReturnsDbSet(inquires);



        }
    }
}
