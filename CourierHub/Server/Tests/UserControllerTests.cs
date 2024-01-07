using Castle.Core.Resource;
using CourierHub.Shared.Controllers;
using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CourierHub.Server.Tests; 
public class UserControllerTests {
    private readonly UserController _controller;

    public UserControllerTest() {
        var mockSet = new Mock<DbSet<User>>();
        var users = new List<User>
        {
            new() { Id = 1, Email = "januszkowalski@gmail.com", Name = "Janusz", Surname = "Kowalski", Type = 1 },
        }.AsQueryable();
        mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
        mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
        mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
        mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

        var mockContext = new Mock<CourierHubDbContext>();
        mockContext.Setup(c => c.Users).Returns(mockSet.Object);
        _controller = new UserController(mockContext.Object);
    }

    [Fact]
    public async Task Head_ShouldReturn200_WhenUserWithEmailExists() {

    }
}
