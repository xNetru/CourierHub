using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Controllers;
using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Test; 
public class OrderControllerTest {
    private readonly OrderController _controller;
    private readonly Mock<CourierHubDbContext> _mockContext;

    public OrderControllerTest() {
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["ServiceName"]).Returns("TestService");

        var mockContext = new Mock<CourierHubDbContext>();
        var address = new Address() { Id = 1, City = "Warszawa" };
        IList<Address> addresses = new List<Address> { address };
        mockContext.Setup(c => c.Addresses).ReturnsDbSet(addresses);

        IList<Inquire> inquires = new List<Inquire> {
            new() { Id = 1, Code = "0123", Datetime = DateTime.Now, Source = address, Destination = address },
            new() { Id = 2, Code = "4567", Datetime = DateTime.Now.AddDays(-9), Source = address, Destination = address },
            new() { Id = 3, Code = "8910", Datetime = DateTime.Now.AddDays(-10), Source = address, Destination = address }
        };
        mockContext.Setup(c => c.Inquires).ReturnsDbSet(inquires);

        IList<Status> statuses = new List<Status> {

            new() { Id = 1, Name = "NotConfirmed", IsCancelable = true },
            new() { Id = 2, Name = "Confirmed", IsCancelable = true },
            new() { Id = 3, Name = "Cancelled", IsCancelable = false },
            new() { Id = 4, Name = "Denied", IsCancelable = false },
            new() { Id = 5, Name = "PickedUp", IsCancelable = false },
            new() { Id = 6, Name = "Delivered", IsCancelable = false },
            new() { Id = 7, Name = "CouldNotDeliver", IsCancelable = false },

        };
        mockContext.Setup(c => c.Statuses).ReturnsDbSet(statuses);

        IList<Service> services = new List<Service> {
            new() { Id = 1, Name = "TestService", ApiKey = "666 - the Number of the Beast" }
        };
        mockContext.Setup(c => c.Services).ReturnsDbSet(services);

        IList<Review> reviews = new List<Review>();
        mockContext.Setup(c => c.Reviews).ReturnsDbSet(reviews);

        IList<Order> orders = new List<Order> {
            new() { Id = 1, InquireId = 1, Inquire = inquires[0], ClientAddressId = 1, StatusId = 1, Service = services[0],
                ClientName = "Janusz", ClientSurname = "Kowalski", ClientEmail =  "januszkowalski@gmail.com",
            },
            new() { Id = 2, InquireId = 2, Inquire = inquires[1], ClientAddressId = 1, StatusId = 1, Service = services[0],
                ClientName = "Maciej", ClientSurname = "Wąsik", ClientEmail =  "maciejwąsik@gmail.com"
            },
            new() { Id = 3, InquireId = 3, Inquire = inquires[2], ClientAddressId = 1, StatusId = 2, Service = services[0],
                ClientName = "Mariusz", ClientSurname = "Kamiński", ClientEmail =  "mariuszkamiński@gmail.com"
            }
        };
        mockContext.Setup(c => c.Orders).ReturnsDbSet(orders);

        _mockContext = mockContext;
        _controller = new OrderController(mockContext.Object, configMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnOrders_WhenNumberOfDaysSpecified() {
        // Arrange
        int days = 10;
        // Act
        var result = await _controller.Get(days);
        // Assert
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        Assert.NotNull(objResult.Value);
        List<ApiOrder> orders = (List<ApiOrder>)objResult.Value;
        Assert.NotEmpty(orders);
        Assert.Equal(2, orders.Count);
    }

    [Fact]
    public async Task GetOrderByStatus_ShouldReturnOrders_WhenProvidedCorrectStatus() {
        // Arrange
        int status = 1;
        // Act
        var result = await _controller.GetOrderByStatus(status);
        // Assert
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        Assert.NotNull(objResult.Value);
        List<ApiOrder> orders = (List<ApiOrder>)objResult.Value;
        Assert.NotEmpty(orders);
        Assert.Equal(2, orders.Count);
    }
}

/*
    // GET: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/status
    [HttpGet("{code}/status")]
    public async Task<ActionResult<ApiStatus>> GetStatusByCode(string code) {
        if (code.IsNullOrEmpty()) { return BadRequest(); }

        var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
        if (order == null) { return NotFound(); }

        var status = await _context.Statuses.FirstOrDefaultAsync(e => e.Id == order.StatusId);
        if (status == null) { return NotFound(); }

        return Ok(new ApiStatus { 
            Name = status.Name, IsCancelable = status.IsCancelable
        });
    }

    // GET: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/service
    [HttpGet("{code}/service")]
    public async Task<ActionResult<string>> GetServiceByCode(string code) {
        if (code.IsNullOrEmpty()) { return BadRequest(); }

        var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
        if (order == null) { return NotFound(); }

        var service = await _context.Services.FirstOrDefaultAsync(e => e.Id == order.ServiceId);
        if (service == null) { return NotFound(); }

        return Ok(service.Name);
    }

    // PATCH: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/status/{...}
    [HttpPatch("{code}/status")]
    public async Task<ActionResult> PatchStatus(string code, [FromBody] StatusType? statusType) {
        if (statusType == null) { return BadRequest(); }
        var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
        if (order == null) { return NotFound(); }
        order.StatusId = (int)statusType;
        await _context.SaveChangesAsync();
        return Ok();
    }

    // PATCH: <OrderController>/q1w2-e3r4-t5y6-u7i8-o9p0/review/{...}
    [HttpPatch("{code}/review")]
    public async Task<ActionResult> PatchReview(string code, [FromBody] ApiReview? review) {
        if (review == null) { return BadRequest(); }

        var order = await _context.Orders.FirstOrDefaultAsync(e => e.Inquire.Code == code);
        if (order == null) { return NotFound(); }

        var reviewDB = (Review)review;
        await _context.Reviews.AddAsync(reviewDB);
        await _context.SaveChangesAsync();

        order.ReviewId = reviewDB.Id;
        await _context.SaveChangesAsync();
        return Ok();
    }
*/