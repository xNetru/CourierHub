using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Controllers;
using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;

namespace CourierHub.Test;
public class InquireControllerTest {
    private readonly InquireController _controller;

    public InquireControllerTest() {
        var mockContext = new Mock<CourierHubDbContext>();
        var address = new Address();
        IList<Inquire> inquires = new List<Inquire> {
            new() { Id = 1, Code = "0123", Datetime = DateTime.Now, Source = address, Destination = address },
            new() { Id = 2, Code = "4567", Datetime = DateTime.Now.AddDays(-9), Source = address, Destination = address },
            new() { Id = 3, Code = "8910", Datetime = DateTime.Now.AddDays(-10), Source = address, Destination = address }
        };
        mockContext.Setup(c => c.Inquires).ReturnsDbSet(inquires);

        _controller = new InquireController(mockContext.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnInquires_WhenNumberOfDaysSpecified() {
        // Arrange
        int days = 10;
        // Act
        var result = await _controller.Get(days);
        // Assert
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        Assert.NotNull(objResult.Value);
        List<ApiInquire> inquires = (List<ApiInquire>)objResult.Value;
        Assert.NotEmpty(inquires);
        Assert.Equal(2, inquires.Count);
    }
}