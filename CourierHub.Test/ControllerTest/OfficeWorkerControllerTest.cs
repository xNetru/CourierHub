using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Controllers;
using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;

namespace CourierHub.Test.ControllerTest;
public class OfficeWorkerControllerTest {
    private readonly OfficeWorkerController _controller;
    private readonly Mock<CourierHubDbContext> _mockContext;

    public OfficeWorkerControllerTest() {
        var mockContext = new Mock<CourierHubDbContext>();
        IList<User> users = new List<User> {
            new() { Id = 1, Email = "januszkowalski@gmail.com", Name = "Janusz", Surname = "Kowalski", Type = 0 },
            new() { Id = 2, Email = "mariuszkamiński@gmail.com", Name = "Mariusz", Surname = "Kamiński", Type = 1 }
        };
        mockContext.Setup(c => c.Users).ReturnsDbSet(users);
        IList<Order> orders = new List<Order> {
            new() { Id = 1, Inquire = new Inquire { Code = "q1w2-e3r4-t5y6-u7i8-o9p0" } }
        };
        mockContext.Setup(c => c.Orders).ReturnsDbSet(orders);
        IList<Evaluation> evaluations = new List<Evaluation>();
        mockContext.Setup(c => c.Evaluations).ReturnsDbSet(evaluations);
        _mockContext = mockContext;
        _controller = new OfficeWorkerController(mockContext.Object);
    }

    [Fact]
    public async Task Head_ShouldReturn200_WhenOfficeWorkerExists() {
        // Arrange
        string email = "mariuszkamiński@gmail.com";
        // Act
        var result = await _controller.Head(email);
        // Assert
        var status = Assert.IsType<OkResult>(result);
        Assert.Equal(200, status.StatusCode);
    }

    [Fact]
    public async Task Head_ShouldReturn404_WhenOfficeWorkerNotExists() {
        // Arrange
        string email = "edytagórniak@gmail.com";
        // Act
        var result = await _controller.Head(email);
        // Assert
        var status = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, status.StatusCode);
    }

    [Fact]
    public async Task Head_ShouldReturn404_WhenUserIsNotOfficeWorker() {
        // Arrange
        string email = "januszkowalski@gmail.com";
        // Act
        var result = await _controller.Head(email);
        // Assert
        var status = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, status.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturnOfficeWorker_WhenOfficeWorkerExists() {
        // Arrange
        string email = "mariuszkamiński@gmail.com";
        // Act
        var result = await _controller.Get(email);
        // Assert
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        ApiOfficeWorker? worker = (ApiOfficeWorker?)objResult.Value;

        Assert.NotNull(worker);
        Assert.Equal("Mariusz", worker.Name);
        Assert.Equal("Kamiński", worker.Surname);
    }

    [Fact]
    public async Task Get_ShouldReturn404_WhenOfficeWorkerNotExists() {
        // Arrange
        string email = "edytagórniak@gmail.com";
        // Act
        var result = await _controller.Get(email);
        // Assert
        NotFoundObjectResult objResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, objResult.StatusCode);
    }

    [Fact]
    public async Task Get_ShouldReturn404_WhenUserIsNotOfficeWorker() {
        // Arrange
        string email = "januszkowalski@gmail.com";
        // Act
        var result = await _controller.Get(email);
        // Assert
        NotFoundObjectResult objResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, objResult.StatusCode);
    }

    [Fact]
    public async Task PatchEvaluation_ShouldAddEvaluation_WhenOfficeWorkerExistsAndEvaluationCorrect() {
        // Arrange
        string email = "mariuszkamiński@gmail.com";
        string code = "q1w2-e3r4-t5y6-u7i8-o9p0";
        var eval = new ApiEvaluation {
            Datetime = DateTime.Today,
            RejectionReason = "Lmao, gottem."
        };
        // Act
        var result = await _controller.PatchEvaluation(email, code, eval);
        // Assert
        var status = Assert.IsType<OkResult>(result);
        Assert.Equal(200, status.StatusCode);
        var order = _mockContext.Object.Orders.FirstOrDefault();
        Assert.NotNull(order);
        Assert.NotNull(order.EvaluationId); // evaluation id was set
    }
}