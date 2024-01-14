using CourierHub.Shared.Controllers;
using CourierHub.Shared.Data;
using CourierHub.Shared.Enums;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;

namespace CourierHub.Test.ControllerTest;
public class UserControllerTest {
    private readonly UserController _controller;

    public UserControllerTest() {
        var mockContext = new Mock<CourierHubDbContext>();
        IList<User> users = new List<User> {
            new() { Id = 1, Email = "januszkowalski@gmail.com", Name = "Janusz", Surname = "Kowalski", Type = 1 },
            new() { Id = 2, Email = "mariuszkamiński@gmail.com", Name = "Mariusz", Surname = "Kamiński", Type = 0 },
        };
        mockContext.Setup(c => c.Users).ReturnsDbSet(users);
        _controller = new UserController(mockContext.Object);
    }


    [Fact]
    public async Task Head_ShouldReturn200_WhenUserExists() {
        // Arrange
        string email = "januszkowalski@gmail.com";
        // Act
        var result = await _controller.Head(email);
        // Assert
        var status = Assert.IsType<OkResult>(result);
        Assert.Equal(200, status.StatusCode);
    }

    [Fact]
    public async Task Head_ShouldReturn404_WhenUserNotExists() {
        // Arrange
        string email = "edytagórniak@gmail.com";
        // Act
        var result = await _controller.Head(email);
        // Assert
        var status = Assert.IsType<NotFoundResult>(result);
        Assert.Equal(404, status.StatusCode);
    }

    [Theory]
    [InlineData("januszkowalski@gmail.com", 1)]
    [InlineData("mariuszkamiński@gmail.com", 0)]
    public async Task GetType_ShouldReturnCorrectType_WhenUserExists(string email, int type) {
        // Act
        var result = await _controller.GetType(email);
        OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, objResult.StatusCode);
        UserType? userType = (UserType?)objResult.Value;
        // Assert
        Assert.NotNull(userType);
        Assert.Equal(type, (int)userType);
    }

    [Fact]
    public async Task GetType_ShouldReturn404_WhenUserNotExists() {
        // Arrange
        string email = "edytagórniak@gmail.com";
        // Act
        var result = await _controller.GetType(email);
        // Assert
        NotFoundObjectResult objResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(404, objResult.StatusCode);
    }
}
