using CourierHub.Server.Api;
using CourierHub.Server.Containers;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Controllers;
using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.EntityFrameworkCore;

namespace CourierHub.Test.ControllerTest {
    public class ApiControllerTest {
        private readonly ApiController _controller;

        public ApiControllerTest() {
            var apiMock = new Mock<IWebApi>();
            var apiOffer = new ApiOffer {
                Code = "0123",
                Price = 1000
            };
            apiMock.SetupGet(c => c.ServiceName).Returns("TestService");
            apiMock.Setup(c => c.PostInquireGetOffer(It.IsAny<ApiInquire>())).Returns(Task.FromResult<(ApiOffer?, int)>((apiOffer, 201)));
            apiMock.Setup(c => c.PostOrder(It.IsAny<ApiOrder>())).Returns(Task.FromResult<(int, string?)>((201, null)));
            apiMock.Setup(c => c.PutOrderWithrawal(It.IsAny<string>())).Returns(Task.FromResult(201));

            var mockContext = new Mock<CourierHubDbContext>();

            var address = new Address() { Id = 1, City = "Warszawa" };
            IList<Address> addresses = new List<Address> { address };
            mockContext.Setup(c => c.Addresses).ReturnsDbSet(addresses);

            IList<User> users = new List<User> {
                new() { Id = 1, Email = "januszkowalski@gmail.com", Name = "Janusz", Surname = "Kowalski", Type = 0 },
            };

            IList<Inquire> inquires = new List<Inquire> {
                new() { Id = 1, Code = "0123", Datetime = DateTime.Now, Source = address, Destination = address },
            };
            mockContext.Setup(c => c.Inquires).ReturnsDbSet(inquires);

            IList<Service> services = new List<Service> {
            new() { Id = 1, Name = "TestService", ApiKey = "666 - the Number of the Beast" }
            };
            mockContext.Setup(c => c.Services).ReturnsDbSet(services);

            IList<Order> orders = new List<Order> {
            new() { Id = 1, InquireId = 1, Inquire = inquires[0], ClientAddressId = 1, ServiceId = 1, Service = services[0],
                ClientName = "Janusz", ClientSurname = "Kowalski", ClientEmail =  "januszkowalski@gmail.com",
            }
            };
            mockContext.Setup(c => c.Orders).ReturnsDbSet(orders);

            var apis = new WebApiContainer(mockContext.Object);
            apis.WebApis.Add(apiMock.Object);

            var inqCode = new InquireCodeContainer();
            inqCode.InquireCodes.Add((new List<string> { "0123" }, 1));

            _controller = new ApiController(mockContext.Object, apis, inqCode);
        }

        [Fact]
        public async Task PostInquireGetOffers_ShouldReturnOffer_WhenOfferReceived() {
            // Arrange
            var inquire = new ApiInquire {
                Datetime = DateTime.Now,
                Source = new ApiAddress { City = "Warszawa" },
                Destination = new ApiAddress { City = "Kraków" }
            };
            // Act
            var result = await _controller.PostInquireGetOffers(inquire);
            // Assert
            OkObjectResult objResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(200, objResult.StatusCode);
            Assert.NotNull(objResult.Value);
            List<ApiOffer> offers = (List<ApiOffer>)objResult.Value;
            Assert.Single(offers);
            var offer = offers.FirstOrDefault();
            Assert.NotNull(offer);
            Assert.Equal("0123", offer.Code);
        }

        [Fact]
        public async Task PostInquireGetOffers_ShouldThrow_WhenInquireNotCorrect() {
            // Arrange
            var inquire = new ApiInquire();
            // Assert
            await Assert.ThrowsAnyAsync<Exception>(async () => await _controller.PostInquireGetOffers(inquire));
        }

        [Fact]
        public async Task PostOrder_ShouldReturn201_WhenOrderAdded() {
            // Arrange
            string serviceName = "TestService";
            var order = new ApiOrder {
                Code = "0123",
                Price = 1000,
                ClientName = "Janusz",
                ClientSurname = "Kowalski",
                ClientEmail = "januszkowalski@gmail.com",
                ClientAddress = new ApiAddress { City = "Warszawa" }
            };
            // Act
            var result = await _controller.PostOrder(serviceName, order);
            // Assert
            var status = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, status.StatusCode);
        }

        [Fact]
        public async Task PostOrder_ShouldReturn404_WhenInquireNotExists() {
            // Arrange
            string serviceName = "TestService";
            var order = new ApiOrder();
            //Act
            var result = await _controller.PostOrder(serviceName, order);
            // Assert
            var status = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, status.StatusCode);
        }

        [Fact]
        public async Task PutOrderWithrawal_ShouldReturn201_WhenOrderIsCancelable() {
            // Arrange
            string code = "0123";
            string serviceName = "TestService";
            // Act
            var result = await _controller.PutOrderWithrawal(serviceName, code);
            // Assert
            var status = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, status.StatusCode);
        }
    }
}
