using CourierHub.Cloud;
using CourierHub.Server.Api;
using CourierHub.Server.Containers;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Controllers;
using CourierHub.Shared.Data;
using CourierHub.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierHub.Test {
    public class ApiControllerTest {
        private readonly ApiController _controller;

        public ApiControllerTest() {
            var inqCodeMock = new Mock<InquireCodeContainer>();
            inqCodeMock.Setup(c => c.InquireCodes).Returns(new List<(List<string>, int)> {
                (new List<string> { "0123" }, 1) 
            });

            var apiMock = new Mock<IWebApi>();
            var apiOffer = new ApiOffer {
                Code = "0123",
                Price = 1000
            };
            apiMock.Setup(c => c.PostInquireGetOffer(It.IsAny<ApiInquire>())).Returns(Task.FromResult<(ApiOffer?, int)>((apiOffer, 201)));
            apiMock.Setup(c => c.PostOrder(It.IsAny<ApiOrder>())).Returns(Task.FromResult<(int, string?)>((201, null)));
            apiMock.Setup(c => c.PutOrderWithrawal(It.IsAny<string>())).Returns(Task.FromResult(201));

            var apisMock = new Mock<WebApiContainer>();
            apisMock.Setup(c => c.WebApis).Returns(new List<IWebApi> { apiMock.Object });

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

            _controller = new ApiController(mockContext.Object, apisMock.Object, inqCodeMock.Object);
        }

        [Fact]
        public async Task PostInquireGetOffers_ShouldReturnOffer_WhenClientExists() {
            // Arrange
            var inquire = new ApiInquire {
                Datetime = DateTime.Now,
                Source = new ApiAddress { City = "Warszawa" },
                Destination = new ApiAddress { City = "Kraków" }
            };
            // Act
            var result = await _controller.PostInquireGetOffers(inquire);
            // Assert
            OkObjectResult status = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, status.StatusCode);
        }

        /*
         * \
         * // POST: <ApiController>/inquire/{...}
    [HttpPost("inquire")]
    public async Task<ActionResult<IEnumerable<ApiOffer>>> PostInquireGetOffers([FromBody] ApiInquire? inquire) {
        if (inquire == null) { return BadRequest(Array.Empty<ApiOffer>()); }

        var offers = new List<ApiOffer>();
        foreach (var webapi in _webApis) {
            (ApiOffer? offer, int status) = await webapi.PostInquireGetOffer(inquire);
            if (offer != null && status >= 200 && status < 300) {
                offers.Add(offer);
            }
        }

        if (offers.Any()) {
            Inquire inquireDB = (Inquire)inquire;
            if (inquire.Email != null) {
                var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == inquire.Email && e.Type == (int)UserType.Client);
                if (user != null) {
                    inquireDB.ClientId = user.Id;
                }
            }
            await _context.Inquires.AddAsync(inquireDB);
            await _context.SaveChangesAsync();

            // cash inquire with codes
            var codeList = offers.Select(e => e.Code).ToList();
            _inquireCodes.Add((codeList, inquireDB.Id));

            return Ok(offers);
        } else {
            return NotFound(Array.Empty<ApiOffer>());
        }
    }

    // POST: <ApiController>/CourierHub/order/{...}
    [HttpPost("{serviceName}/order")]
    public async Task<ActionResult> PostOrder(string serviceName, [FromBody] ApiOrder? order) {
        if (order == null) { return BadRequest(); }

        var service = await _context.Services.FirstOrDefaultAsync(e => e.Name == serviceName);
        if (service == null) { return NotFound(); }

        foreach (var webapi in _webApis) {
            if (webapi.ServiceName == serviceName) {
                (int status, string? code) = await webapi.PostOrder(order);

                // retrieve cashed id
                int inquireId = _inquireCodes.FirstOrDefault(e => e.Item1.Contains(order.Code)).Item2;

                var inquireDB = _context.Inquires.FirstOrDefault(e => e.Id == inquireId);
                if (inquireDB == null) { return NotFound(); }
                if (code != null) {
                    inquireDB.Code = code;
                } else {
                    inquireDB.Code = order.Code;
                }

                if (status >= 200 && status < 300) {
                    Order orderDB = (Order)order;
                    orderDB.InquireId = inquireId;
                    orderDB.ServiceId = service.Id;
                    orderDB.StatusId = (int)StatusType.NotConfirmed;
                    await _context.Orders.AddAsync(orderDB);
                }
                await _context.SaveChangesAsync();

                return StatusCode(status);
            }
        }
        return NotFound(); // should not happen if serviceName exists
    }

    // PATCH: <ApiController>/CourierHub/cancel/q1w2-e3r4-t5y6-u7i8-o9p0
    [HttpPatch("{serviceName}/cancel/{code}")]
    public async Task<ActionResult> PutOrderWithrawal(string serviceName, string code) {
        foreach (var webapi in _webApis) {
            if (webapi.ServiceName == serviceName) {
                int status = await webapi.PutOrderWithrawal(code);
                return StatusCode(status);
            }
        }
        return NotFound(); // should not happen if serviceName exists
    }*/
    }
}
