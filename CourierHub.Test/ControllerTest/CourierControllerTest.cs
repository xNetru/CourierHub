﻿using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Controllers;
using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;

namespace CourierHub.Test.ControllerTest;
public class CourierControllerTest {
    private readonly CourierController _controller;
    private readonly Mock<CourierHubDbContext> _mockContext;

    public CourierControllerTest() {
        var mockContext = new Mock<CourierHubDbContext>();
        IList<Address> addresses = new List<Address> {
            new() { Id = 1, City = "Warszawa" },
        };
        mockContext.Setup(c => c.Addresses).ReturnsDbSet(addresses);
        IList<User> users = new List<User> {
            new() { Id = 1, Email = "januszkowalski@gmail.com", Name = "Janusz", Surname = "Kowalski", Type = 0 },
            new() { Id = 2, Email = "mariuszkamiński@gmail.com", Name = "Mariusz", Surname = "Kamiński", Type = 1 },
            new() { Id = 3, Email = "maciejwąsik@gmail.com", Name = "Maciej", Surname = "Wąsik", Type = 2 }
        };
        mockContext.Setup(c => c.Users).ReturnsDbSet(users);
        IList<Inquire> inquires = new List<Inquire> {
            new() { Id = 1, Code = "q1w2-e3r4-t5y6-u7i8-o9p0" },
            new() { Id = 2, Code = "0123" }
        };
        mockContext.Setup(c => c.Inquires).ReturnsDbSet(inquires);
        IList<Order> orders = new List<Order> {
            new() { Id = 1, Inquire = inquires[0] },
            new() { Id = 2, InquireId = 2, Inquire = inquires[1], Parcel = new Parcel {
                    Id = 2, CourierId = 2, Courier = users[2]
                }, ParcelId = 2, ClientAddressId = 1, ClientAddress = addresses[0], StatusId = 5
            }
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
        // Assert
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        ApiCourier? courier = (ApiCourier?)objResult.Value;

        Assert.NotNull(courier);
        Assert.Equal("Maciej", courier.Name);
        Assert.Equal("Wąsik", courier.Surname);
    }

    [Fact]
    public async Task Get_ShouldReturn404_WhenCourierNotExists() {
        // Arrange
        string email = "edytagórniak@gmail.com";
        // Act
        var result = await _controller.Get(email);
        // Assert
        NotFoundObjectResult objResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, objResult.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturn404_WhenUserIsNotCourier() {
        // Arrange
        string email = "januszkowalski@gmail.com";
        // Act
        var result = await _controller.Get(email);
        // Assert
        NotFoundObjectResult objResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, objResult.StatusCode);
    }

    [Fact]
    public async Task PatchParcel_ShouldAddParcel_WhenCourierExistsAndParcelCorrect() {
        // Arrange
        string email = "maciejwąsik@gmail.com";
        string code = "q1w2-e3r4-t5y6-u7i8-o9p0";
        var parcel = new ApiParcel {
            PickupDatetime = DateTime.Now,
            DeliveryDatetime = DateTime.Now.AddDays(1),
            UndeliveredReason = "Pozdrowienia do Więzienia"
        };
        int orderStatus = 1;
        // Act
        var result = await _controller.PatchParcel(email, code, orderStatus, parcel);
        // Assert
        var status = Assert.IsType<OkResult>(result);
        Assert.Equal(200, status.StatusCode);
        var order = _mockContext.Object.Orders.FirstOrDefault();
        Assert.NotNull(order);
        Assert.NotNull(order.ParcelId); // parcel id was set
    }

    [Fact]
    public async Task GetOrders_ShouldReturnOrders_WhenThereIsCourierWithParcel() {
        // Arrange
        string email = "maciejwąsik@gmail.com";
        // Act
        var result = await _controller.GetOrders(email);
        // Assert
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        Assert.NotNull(objResult.Value);
        List<ApiOrder> orders = (List<ApiOrder>)objResult.Value;
        Assert.NotEmpty(orders);
        Assert.Single(orders);
    }

    [Fact]
    public async Task GetOrders_ShouldReturn404_WhenCourierNotExists() {
        // Arrange
        string email = "edytagórniak@gmail.com";
        // Act
        var result = await _controller.GetOrders(email);
        // Assert
        NotFoundObjectResult objResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, objResult.StatusCode);
    }
}