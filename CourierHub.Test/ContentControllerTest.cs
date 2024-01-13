using CourierHub.Cloud;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Controllers;
using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;

namespace CourierHub.Test;
public class ContentControllerTest {
    private readonly ContentController _controller;

    public ContentControllerTest() {
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(c => c["ServiceName"]).Returns("TestService");

        var storageMock = new Mock<ICloudStorage>();
        storageMock.SetReturnsDefault(Task.CompletedTask);

        var communicationMock = new Mock<ICloudCommunicationService>();
        communicationMock.SetReturnsDefault(Task.CompletedTask);

        var mockContext = new Mock<CourierHubDbContext>();
        IList<Service> services = new List<Service> {
            new() { Id = 1, Name = "TestService", ApiKey = "666 - the Number of the Beast" }
        };
        mockContext.Setup(c => c.Services).ReturnsDbSet(services);

        _controller = new ContentController(mockContext.Object, configMock.Object, storageMock.Object, communicationMock.Object);
    }

    [Fact]
    public async Task PostContract_ShouldReturn200_WhenContractCorrect() {
        // Arrange
        var contract = new ApiContract {
            DateTime = DateTime.Now,
            Code = "0123",
            Client = new ApiClient {
                Name = "Mariusz",
                Surname = "Kamiński",
                Email = "mariuszkamiński@gmail.com",
                Address = new ApiAddress { City = "Warszawa" },
                SourceAddress = new ApiAddress { City = "Radom" }
            }
        };
        // Act
        var result = await _controller.PostContract(contract);
        // Assert
        var status = Assert.IsType<OkResult>(result);
        Assert.Equal(200, status.StatusCode);
    }

    [Fact]
    public async Task PostContract_ShouldThrow_WhenContractNotCorrect() {
        // Arrange
        var contract = new ApiContract();
        // Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => await _controller.PostContract(contract));
    }

    [Fact]
    public async Task PostReceipt_ShouldReturn200_WhenReceiptCorrect() {
        // Arrange
        var receipt = new ApiReceipt {
            DateTime = DateTime.Now,
            Code = "0123",
            Inquire = new ApiInquire {
                Datetime = DateTime.Now,
                Source = new ApiAddress { City = "Warszawa" },
                Destination = new ApiAddress { City = "Radom" }
            },
            Order = new ApiOrder {
                ClientName = "Mariusz",
                ClientSurname = "Kamiński",
                ClientEmail = "mariuszkamiński@gmail.com",
                ClientAddress = new ApiAddress { City = "Radom" },
                ClientCompany = "Zakład Karny Radom",
            }
        };
        // Act
        var result = await _controller.PostReceipt(receipt);
        // Assert
        var status = Assert.IsType<OkResult>(result);
        Assert.Equal(200, status.StatusCode);
    }

    [Fact]
    public async Task PostReceipt_ShouldThrow_WhenReceiptNotCorrect() {
        // Arrange
        var receipt = new ApiReceipt();
        // Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => await _controller.PostReceipt(receipt));
    }

    /*
    [Fact]
    public async Task PostMailContent_ShouldReturn200_WhenReceiptCorrect() {
        // Arrange
        var client = new ApiClient {
            Name = "Mariusz",
            Surname = "Kamiński",
            Email = "mariuszkamiński@gmail.com",
            Address = new ApiAddress { City = "Warszawa" },
            SourceAddress = new ApiAddress { City = "Radom" }
        };
        var content = new ApiMailContent {
            Client = client,
            Contract = new ApiContract {
                DateTime = DateTime.Now,
                Code = "0123",
                Client = client
            },
            Receipt = new ApiReceipt {
                DateTime = DateTime.Now,
                Code = "0123",
                Inquire = new ApiInquire {
                    Datetime = DateTime.Now,
                    Source = new ApiAddress { City = "Warszawa" },
                    Destination = new ApiAddress { City = "Radom" }
                },
                Order = new ApiOrder {
                    ClientName = "Mariusz",
                    ClientSurname = "Kamiński",
                    ClientEmail = "mariuszkamiński@gmail.com",
                    ClientAddress = new ApiAddress { City = "Radom" },
                    ClientCompany = "Zakład Karny Radom",
                }
            },
            Link = "https://www.youtube.com/watch?v=dQw4w9WgXcQ",
            Recipient = "mariuszkamiński@gmail.com"
        };
        // Act
        var result = await _controller.PostMailContent(content);
        // Assert
        var status = Assert.IsType<OkResult>(result);
        Assert.Equal(200, status.StatusCode);
    }
    */

    [Fact]
    public async Task PostMailContent_ShouldThrow_WhenReceiptNotCorrect() {
        // Arrange
        var content = new ApiMailContent();
        // Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => await _controller.PostMailContent(content));
    }
}
