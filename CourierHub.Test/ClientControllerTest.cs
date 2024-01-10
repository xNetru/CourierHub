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
public class ClientControllerTest {
    private readonly ClientController _controller;
    private readonly Mock<CourierHubDbContext> _mockContext;

    public ClientControllerTest() {
        var mockContext = new Mock<CourierHubDbContext>();
        IList<User> users = new List<User> {
            new() { Id = 1, Email = "januszkowalski@gmail.com", Name = "Janusz", Surname = "Kowalski", Type = 0 },
            new() { Id = 2, Email = "mariuszkamiński@gmail.com", Name = "Mariusz", Surname = "Kamiński", Type = 1 }
        };
        mockContext.Setup(c => c.Users).ReturnsDbSet(users);
        IList<Address> addresses = new List<Address> {
            new() { Id = 1, City = "Warszawa" },
            new() { Id = 2, City = "Kraków" },
        };
        mockContext.Setup(c => c.Addresses).ReturnsDbSet(addresses);
        IList<ClientData> datum = new List<ClientData> {
            new() { Id = 1, ClientId = 1, SourceAddressId = 1, AddressId = 2 }
        };
        mockContext.Setup(c => c.ClientDatum).ReturnsDbSet(datum);
        IList<Inquire> inquires = new List<Inquire> { 
            new() { Id = 1, Code = "q1w2-e3r4-t5y6-u7i8-o9p0", ClientId = 1, SourceId = 1, DestinationId = 2 }
        };
        mockContext.Setup(c => c.Inquires).ReturnsDbSet(inquires);
        IList<Order> orders = new List<Order> {
            new() { Id = 1, InquireId = 1, ClientAddressId = 2,
                ClientName = "Janusz", ClientSurname = "Kowalski", ClientEmail =  "januszkowalski@gmail.com"
            }
        };
        mockContext.Setup(c => c.Orders).ReturnsDbSet(orders);
        _mockContext = mockContext;
        _controller = new ClientController(mockContext.Object);
    }

    [Fact]
    public async Task Head_ShouldReturn200_WhenClientExists() {
        // Arrange
        string email = "januszkowalski@gmail.com";
        // Act
        var result = await _controller.Head(email);
        // Assert
        var status = Assert.IsType<OkResult>(result);
        Assert.Equal(200, status.StatusCode);
    }

    [Fact]
    public async Task Head_ShouldReturn404_WhenClientNotExists() {
        // Arrange
        string email = "edytagórniak@gmail.com";
        // Act
        var result = await _controller.Head(email);
        // Assert
        var status = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, status.StatusCode);
    }

    [Fact]
    public async Task Head_ShouldReturn404_WhenUserIsNotClient() {
        // Arrange
        string email = "mariuszkamiński@gmail.com";
        // Act
        var result = await _controller.Head(email);
        // Assert
        var status = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, status.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnClient_WhenClientsExists() {
        // Arrange
        string email = "januszkowalski@gmail.com";
        // Act
        var result = await _controller.Get(email);
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        ApiClient? client = (ApiClient?)objResult.Value;
        // Assert
        Assert.NotNull(client);
        Assert.Equal("Janusz", client.Name);
        Assert.Equal("Kowalski", client.Surname);
        Assert.Equal("Kraków", client.Address.City);
    }

    [Fact]
    public async Task Get_ShouldReturn404_WhenClienntNotExists() {
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
        string email = "mariuszkamiński@gmail.com";
        // Act
        var result = await _controller.Get(email);
        // Assert
        NotFoundObjectResult objResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, objResult.StatusCode);
    }
}