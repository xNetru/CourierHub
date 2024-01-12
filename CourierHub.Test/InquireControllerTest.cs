using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Controllers;
using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Test; 
public class InquireControllerTest {
    private readonly InquireControllerTest _controller;
    private readonly Mock<CourierHubDbContext> _mockContext;

    public InquireControllerTest() {
        var mockContext = new Mock<CourierHubDbContext>();
        IList<User> users = new List<User> {
            new() { Id = 1, Email = "januszkowalski@gmail.com", Name = "Janusz", Surname = "Kowalski", Type = 0 },
            new() { Id = 2, Email = "mariuszkamiński@gmail.com", Name = "Mariusz", Surname = "Kamiński", Type = 1 },
            new() { Id = 3, Email = "maciejwąsik@gmail.com", Name = "Maciej", Surname = "Wąsik", Type = 2 }
        };
        mockContext.Setup(c => c.Users).ReturnsDbSet(users);
        IList<Order> orders = new List<Order> {
            new() { Id = 1, Inquire = new Inquire { Code = "q1w2-e3r4-t5y6-u7i8-o9p0" } }
        };
        mockContext.Setup(c => c.Orders).ReturnsDbSet(orders);
        IList<Parcel> parcels = new List<Parcel>();
        mockContext.Setup(c => c.Parcels).ReturnsDbSet(parcels);
        _mockContext = mockContext;
        _controller = new CourierController(mockContext.Object);
    }

    [Fact]
    public async Task Head_ShouldReturn200_WhenCourierExists() {
        // Arrange
        string email = "maciejwąsik@gmail.com";
        // Act
        var result = await _controller.Head(email);
        // Assert
        var status = Assert.IsType<OkResult>(result);
        Assert.Equal(200, status.StatusCode);
    }

    [Fact]
    public async Task Head_ShouldReturn404_WhenCourierNotExists() {
        // Arrange
        string email = "edytagórniak@gmail.com";
        // Act
        var result = await _controller.Head(email);
        // Assert
        var status = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, status.StatusCode);
    }

    [Fact]
    public async Task Head_ShouldReturn404_WhenUserIsNotCourier() {
        // Arrange
        string email = "januszkowalski@gmail.com";
        // Act
        var result = await _controller.Head(email);
        // Assert
        var status = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, status.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnCourier_WhenCourierExists() {
        // Arrange
        string email = "maciejwąsik@gmail.com";
        // Act
        var result = await _controller.Get(email);
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        ApiCourier? courier = (ApiCourier?)objResult.Value;
        // Assert
        Assert.NotNull(courier);
        Assert.Equal("Maciej", courier.Name);
        Assert.Equal("Wąsik", courier.Surname);
    }

    [Fact]
    public async Task Get_ShouldReturn404_WhenClientNotExists() {
        // Arrange
        string email = "edytagórniak@gmail.com";
        // Act
        var result = await _controller.Get(email);
        // Assert
        NotFoundObjectResult objResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, objResult.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturn404_WhenUserIsNotClient() {
        // Arrange
        string email = "januszkowalski@gmail.com";
        // Act
        var result = await _controller.Get(email);
        // Assert
        NotFoundObjectResult objResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, objResult.StatusCode);
    }

    [Fact]
    public async Task PatchParcel_ShouldAddEvaluation_WhenCourierExistsAndParcelCorrect() {
        // Arrange
        string email = "maciejwąsik@gmail.com";
        string code = "q1w2-e3r4-t5y6-u7i8-o9p0";
        var parcel = new ApiParcel {
            PickupDatetime = DateTime.Now,
            DeliveryDatetime = DateTime.Now.AddDays(1),
            UndeliveredReason = "Pozdrowienia do Więzienia"
        };
        // Act
        var result = await _controller.PatchParcel(email, code, parcel);
        // Assert
        var status = Assert.IsType<OkResult>(result);
        Assert.Equal(200, status.StatusCode);
        var order = _mockContext.Object.Orders.FirstOrDefault();
        Assert.NotNull(order);
        Assert.NotNull(order.ParcelId); // parcel id was set
    }
}