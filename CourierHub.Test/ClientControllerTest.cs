using Azure.Communication.Email;
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
using System.Net;
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
            new() { Id = 1, ClientId = 1, SourceAddress = addresses[0], SourceAddressId = 1, AddressId = 2, Address = addresses[1] }
        };
        mockContext.Setup(c => c.ClientDatum).ReturnsDbSet(datum);
        IList<Inquire> inquires = new List<Inquire> {
            new() { Id = 1, Code = "0123", ClientId = 1, SourceId = 1, DestinationId = 2, Datetime = DateTime.Now },
            new() { Id = 2, Code = "4567", ClientId = 1, SourceId = 1, DestinationId = 2, Datetime = DateTime.Now.AddDays(-9) },
            new() { Id = 3, Code = "8910", ClientId = 1, SourceId = 1, DestinationId = 2, Datetime = DateTime.Now.AddDays(-10) }
        };
        mockContext.Setup(c => c.Inquires).ReturnsDbSet(inquires);
        IList<Order> orders = new List<Order> {
            new() { Id = 1, InquireId = 1, Inquire = inquires[0], ClientAddressId = 2,
                ClientName = "Janusz", ClientSurname = "Kowalski", ClientEmail =  "januszkowalski@gmail.com"
            },
            new() { Id = 2, InquireId = 2, Inquire = inquires[1], ClientAddressId = 2,
                ClientName = "Janusz", ClientSurname = "Kowalski", ClientEmail =  "januszkowalski@gmail.com"
            },
            new() { Id = 3, InquireId = 3, Inquire = inquires[2], ClientAddressId = 2,
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
        // Assert
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        ApiClient? client = (ApiClient?)objResult.Value;

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

    [Fact]
    public async Task GetInquires_ShouldReturnInquires_WhenNumberOfDaysSpecified() {
        // Arrange
        string email = "januszkowalski@gmail.com";
        int days = 10;
        // Act
        var result = await _controller.GetInquires(email, days);
        // Assert
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        Assert.NotNull(objResult.Value);
        List<ApiInquire> inquires = (List<ApiInquire>)objResult.Value;
        Assert.NotEmpty(inquires);
        Assert.Equal(2, inquires.Count);
    }

    [Fact]
    public async Task GetOrders_ShouldReturnOrders_WhenNumberOfDaysSpecified() {
        // Arrange
        string email = "januszkowalski@gmail.com";
        int days = 10;
        // Act
        var result = await _controller.GetOrders(email, days);
        // Assert
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        Assert.NotNull(objResult.Value);
        List<ApiOrder> inquires = (List<ApiOrder>)objResult.Value;
        Assert.NotEmpty(inquires);
        Assert.Equal(2, inquires.Count);
    }

    [Fact]
    public async Task Put_ShouldChangeClient_WhenClientExists() {
        // Arrange
        string email = "januszkowalski@gmail.com";
        var client = new ApiClient {
            Name = "Nie",
            Surname = "Istnieje",
            Email = email,
            Address = new ApiAddress(),
            SourceAddress = new ApiAddress(),
        };
        // Act
        var result = await _controller.Put(email, client);
        // Assert
        var status = Assert.IsType<OkResult>(result);
        Assert.Equal(200, status.StatusCode);
        var user = _mockContext.Object.Users.FirstOrDefault(e => e.Email == email);
        Assert.NotNull(user);
        Assert.Equal("Nie", user.Name);
    }

    [Fact]
    public async Task Put_ShouldAddClient_WhenClientNotExists() {
        // Arrange
        string email = "maciejwąsik@gmail.com";
        var client = new ApiClient {
            Name = "Maciej",
            Surname = "Wąsik",
            Email = email,
            Address = new ApiAddress(),
            SourceAddress = new ApiAddress(),
            Company = "Zakład Karny Ostrołęka"
        };
        // Act
        var result = await _controller.Put(email, client);
        // Assert
        var status = Assert.IsType<OkResult>(result);
        Assert.Equal(200, status.StatusCode);
    }

    [Fact]
    public async Task Post_ShouldAddClient_Always() {
        // Arrange
        var client = new ApiClient {
            Name = "Maciej",
            Surname = "Wąsik",
            Email = "maciejwąsik@gmail.com",
            Address = new ApiAddress(),
            SourceAddress = new ApiAddress(),
            Company = "Zakład Karny Ostrołęka"
        };
        // Act
        var result = await _controller.Post(client);
        // Assert
        var status = Assert.IsType<OkResult>(result);
        Assert.Equal(200, status.StatusCode);
    }
}