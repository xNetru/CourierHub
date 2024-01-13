using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Controllers;
using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;

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

        var evaluation = new Evaluation {
            Id = 1,
            Datetime = DateTime.Now,
            OfficeWorkerId = 2,
            RejectionReason = "REASON"
        };
        IList<Evaluation> evaluations = new List<Evaluation> { evaluation };
        mockContext.Setup(c => c.Evaluations).ReturnsDbSet(evaluations);

        var parcel = new Parcel {
            Id = 1,
            PickupDatetime = DateTime.Now,
            DeliveryDatetime = DateTime.Now.AddDays(1),
            CourierId = 3,
            UndeliveredReason = "REASON"
            
        };
        IList<Parcel> parcels = new List<Parcel> { parcel };
        mockContext.Setup(c => c.Parcels).ReturnsDbSet(parcels);

        var review = new Review {
            Id = 1,
            Datetime = DateTime.Now,
            Value = 5,
            Description = "REASON"
        };
        IList<Review> reviews = new List<Review> { review };
        mockContext.Setup(c => c.Reviews).ReturnsDbSet(reviews);

        IList<Order> orders = new List<Order> {
            new() { Id = 1, InquireId = 1, Inquire = inquires[0], ClientAddressId = 1, StatusId = 1, ServiceId = 1, Service = services[0],
                ClientName = "Janusz", ClientSurname = "Kowalski", ClientEmail =  "januszkowalski@gmail.com",
            },
            new() { Id = 2, InquireId = 2, Inquire = inquires[1], ClientAddressId = 1, StatusId = 1, ServiceId = 1, Service = services[0],
                ClientName = "Maciej", ClientSurname = "Wąsik", ClientEmail =  "maciejwąsik@gmail.com"
            },
            new() { Id = 3, InquireId = 3, Inquire = inquires[2], ClientAddressId = 1, StatusId = 2, ServiceId = 1, Service = services[0],
                ClientName = "Mariusz", ClientSurname = "Kamiński", ClientEmail =  "mariuszkamiński@gmail.com",
                Evaluation= evaluation, EvaluationId = 1, Parcel = parcel, ParcelId = 1, Review = review, ReviewId = 1,
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

    [Fact]
    public async Task GetStatusByCode_ShouldReturnStatus_WhenOrderExists() {
        // Arrange
        string code = "0123";
        // Act
        var result = await _controller.GetStatusByCode(code);
        // Assert
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        Assert.NotNull(objResult.Value);
        ApiStatus status = (ApiStatus)objResult.Value;
        Assert.NotNull(status);
        Assert.Equal("NotConfirmed", status.Name);
    }

    [Fact]
    public async Task GetStatusByCode_ShouldReturn404_WhenOrderNotExists() {
        // Arrange
        string code = "ABCD";
        // Act
        var result = await _controller.GetStatusByCode(code);
        // Assert
        NotFoundResult res = Assert.IsType<NotFoundResult>(result.Result);
        Assert.Equal(404, res.StatusCode);
    }

    [Fact]
    public async Task GetEvaluationByCode_ShouldReturnEvaluation_WhenOrderExists() {
        // Arrange
        string code = "8910";
        // Act
        var result = await _controller.GetEvaluationByCode(code);
        // Assert
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        Assert.NotNull(objResult.Value);
        ApiEvaluation evaluation = (ApiEvaluation)objResult.Value;
        Assert.NotNull(evaluation);
        Assert.Equal("REASON", evaluation.RejectionReason);
    }

    [Fact]
    public async Task GetEvaluationByCode_ShouldReturn404_WhenOrderNotExists() {
        // Arrange
        string code = "ABCD";
        // Act
        var result = await _controller.GetEvaluationByCode(code);
        // Assert
        NotFoundResult res = Assert.IsType<NotFoundResult>(result.Result);
        Assert.Equal(404, res.StatusCode);
    }

    [Fact]
    public async Task GetParcelByCode_ShouldReturnParcel_WhenOrderExists() {
        // Arrange
        string code = "8910";
        // Act
        var result = await _controller.GetParcelByCode(code);
        // Assert
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        Assert.NotNull(objResult.Value);
        ApiParcel parcel = (ApiParcel)objResult.Value;
        Assert.NotNull(parcel);
        Assert.Equal("REASON", parcel.UndeliveredReason);
    }

    [Fact]
    public async Task GetParcelByCode_ShouldReturn404_WhenOrderNotExists() {
        // Arrange
        string code = "ABCD";
        // Act
        var result = await _controller.GetParcelByCode(code);
        // Assert
        NotFoundResult res = Assert.IsType<NotFoundResult>(result.Result);
        Assert.Equal(404, res.StatusCode);
    }

    [Fact]
    public async Task GetReviewByCode_ShouldReturnReview_WhenOrderExists() {
        // Arrange
        string code = "8910";
        // Act
        var result = await _controller.GetReviewByCode(code);
        // Assert
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        Assert.NotNull(objResult.Value);
        ApiReview review = (ApiReview)objResult.Value;
        Assert.NotNull(review);
        Assert.Equal("REASON", review.Description);
    }

    [Fact]
    public async Task GetReviewByCode_ShouldReturn404_WhenOrderNotExists() {
        // Arrange
        string code = "ABCD";
        // Act
        var result = await _controller.GetReviewByCode(code);
        // Assert
        NotFoundResult res = Assert.IsType<NotFoundResult>(result.Result);
        Assert.Equal(404, res.StatusCode);
    }

    [Fact]
    public async Task GetServiceByCode_ShouldReturnService_WhenOrderExists() {
        // Arrange
        string code = "0123";
        // Act
        var result = await _controller.GetServiceByCode(code);
        // Assert
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        Assert.NotNull(objResult.Value);
        string serviceName = (string)objResult.Value;
        Assert.NotNull(serviceName);
        Assert.Equal("TestService", serviceName);
    }

    [Fact]
    public async Task GetServiceByCode_ShouldReturn404_WhenOrderNotExists() {
        // Arrange
        string code = "ABCD";
        // Act
        var result = await _controller.GetServiceByCode(code);
        // Assert
        NotFoundResult res = Assert.IsType<NotFoundResult>(result.Result);
        Assert.Equal(404, res.StatusCode);
    }

    [Fact]
    public async Task PatchStatus_ShouldChnageStatus_WhenOrderExists() {
        // Arrange
        string code = "0123";
        StatusType s = StatusType.Delivered;
        // Act
        var result = await _controller.PatchStatus(code, s);
        // Assert
        OkResult res = Assert.IsType<OkResult>(result);
        Assert.Equal(200, res.StatusCode);
        var order = _mockContext.Object.Orders.FirstOrDefault(e => e.Inquire.Code == code);
        Assert.NotNull(order);
        Assert.Equal((int)s, order.StatusId);
    }

    [Fact]
    public async Task PatchStatus_ShouldReturn404_WhenOrderNotExists() {
        // Arrange
        string code = "ABCD";
        StatusType s = StatusType.Delivered;
        // Act
        var result = await _controller.PatchStatus(code, s);
        // Assert
        NotFoundResult res = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, res.StatusCode);
    }

    [Fact]
    public async Task PatchReview_ShouldAddReview_WhenOrderExists() {
        // Arrange
        string code = "0123";
        var review = new ApiReview {
            Datetime = DateTime.Now,
            Value = 2,
            Description = "Mocne 2/10."
        };
        // Act
        var result = await _controller.PatchReview(code, review);
        // Assert
        var status = Assert.IsType<OkResult>(result);
        Assert.Equal(200, status.StatusCode);
        var order = _mockContext.Object.Orders.FirstOrDefault(e => e.Inquire.Code == code);
        Assert.NotNull(order);
        Assert.NotNull(order.ReviewId); // review id was set
    }

    [Fact]
    public async Task PatchReview_ShouldReturn404_WhenOrderNotExists() {
        // Arrange
        string code = "ABCD";
        var review = new ApiReview {
            Datetime = DateTime.Now,
            Value = 2,
            Description = "Mocne 2/10."
        };
        // Act
        var result = await _controller.PatchReview(code, review);
        // Assert
        NotFoundResult res = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, res.StatusCode);
    }
}