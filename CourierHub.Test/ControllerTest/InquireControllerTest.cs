using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Controllers;
using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;

namespace CourierHub.Test.ControllerTest;
public class InquireControllerTest {
    private readonly InquireController _controller;

    public InquireControllerTest() {
        var mockContext = new Mock<CourierHubDbContext>();
        var address = new Address {
            Id = 1,
            City = "Warszawa"
        };
        IList<Address> addresses = new List<Address> { address };
        mockContext.Setup(c => c.Addresses).ReturnsDbSet(addresses);
        IList<Inquire> inquires = new List<Inquire> {
            new() { Id = 1, Code = "0123", Datetime = DateTime.Now, SourceId = 1, Source = address,
                DestinationId = 1, Destination = address, Mass = 1000
            },
            new() { Id = 2, Code = "4567", Datetime = DateTime.Now.AddDays(-9), SourceId = 1, Source = address,
                DestinationId = 1, Destination = address
            },
            new() { Id = 3, Code = "8910", Datetime = DateTime.Now.AddDays(-10), SourceId = 1, Source = address,
                DestinationId = 1, Destination = address
            }
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

    [Fact]
    public async Task Get_ShouldReturnInquire_WhenInquireExists() {
        // Arrange
        string code = "0123";
        // Act
        var result = await _controller.GetInquireByCode(code);
        // Assert
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        Assert.NotNull(objResult.Value);
        ApiInquire inquire = (ApiInquire)objResult.Value;
        Assert.NotNull(inquire);
        Assert.Equal(1000, inquire.Mass);
    }

    [Fact]
    public async Task Get_ShouldReturn404_WhenInquireNotExists() {
        // Arrange
        string code = "ABCD";
        // Act
        var result = await _controller.GetInquireByCode(code);
        // Assert
        NotFoundResult res = Assert.IsType<NotFoundResult>(result.Result);
        Assert.Equal(404, res.StatusCode);
    }
}