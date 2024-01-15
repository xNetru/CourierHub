using Bunit;
using Bunit.TestDoubles;
using CourierHub.Client.Data;
using CourierHub.Client.Pages;
using CourierHub.Shared.ApiModels;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;
using System.Security.Claims;

namespace CourierHub.Test.FrontendTest
{
    public class OfficeWorkerReviewOrderTest
    {
        [Fact]
        public void RendersCorrectly()
        {
            // Arrange
            var orderCode = "123";
            var mail = "test@mail.com";
            var name = "Skam";

            using var ctx = new TestContext();

            OrderContainer container = new OrderContainer();
            container.Orders.Add(new ApiOrder { ClientName = name, Code = orderCode, ClientAddress = new ApiAddress { Number = "", Flat = "" }, ClientPhone = "", ClientEmail = mail });
            ctx.Services.AddSingleton(container);

            var httpClient = ctx.Services.AddMockHttpClient();
            httpClient
                .When($"http://localhost:7008/Inquire/{orderCode}/code")
                .RespondJson(new ApiInquire { Source = new ApiAddress { Number = "", Flat = "" }, Destination = new ApiAddress { Number = "", Flat = "" } });

            httpClient
                .When($"http://localhost:7008/Client/{mail}")
                .RespondJson(new ApiClient { Phone = "", Address = new ApiAddress { Number = "", Flat = "" }, SourceAddress = new ApiAddress { Number = "", Flat = "" } });

            // Act
            var component = ctx.RenderComponent<OfficeWorkerReviewOrder>();

            // Assert
            component.WaitForAssertion(() => component.Find(".form-control").TextContent.Equals(name));
            Assert.Equal("Przegląd oferty", component.Find($"h1").TextContent);
        }

        [Fact]
        public void AcceptCorrectly()
        {
            // Arrange
            var orderCode = "123";
            var mail = "test@mail.com";
            var name = "Skam";

            using var ctx = new TestContext();

            OrderContainer container = new OrderContainer();
            container.Orders.Add(new ApiOrder { ClientName = name, Code = orderCode, ClientAddress = new ApiAddress { Number = "", Flat = "" }, ClientPhone = "", ClientEmail = mail });
            ctx.Services.AddSingleton(container);

            var httpClient = ctx.Services.AddMockHttpClient();
            httpClient
                .When($"http://localhost:7008/Inquire/{orderCode}/code")
                .RespondJson(new ApiInquire { Source = new ApiAddress { Number = "", Flat = "" }, Destination = new ApiAddress { Number = "", Flat = "" } });

            httpClient
                .When($"http://localhost:7008/Client/{mail}")
                .RespondJson(new ApiClient { Phone = "", Address = new ApiAddress { Number = "", Flat = "" }, SourceAddress = new ApiAddress { Number = "", Flat = "" } });

            httpClient
                .When($"http://localhost:7008/Order/{orderCode}/status")
                .SendJson();

            httpClient
                .When($"http://localhost:7008/Content/mailcontent")
                .SendJson();

            httpClient
                .When($"http://localhost:7008/Content/receipt")
                .SendJson();

            httpClient
                .When($"http://localhost:7008/Content/contract")
                .SendJson();

            // Act
            var component = ctx.RenderComponent<OfficeWorkerReviewOrder>();

            // Assert
            component.WaitForAssertion(() => component.Find(".form-control").TextContent.Equals(name));
            Assert.Equal("Przegląd oferty", component.Find($"h1").TextContent);
            var buttons = component.FindAll(".btn");
            buttons[0].Click();
            component.WaitForAssertion(() => component.Find(".p-2").TextContent.Equals("Oferta zaakceptowana"));
        }

        [Fact]
        public void DenyCorrectly()
        {
            // Arrange
            var orderCode = "123";
            var mail = "test@mail.com";
            var worker_mail = "worker@mail.com";
            var name = "Skam";

            using var ctx = new TestContext();

            OrderContainer container = new OrderContainer();
            container.Orders.Add(new ApiOrder { ClientName = name, Code = orderCode, ClientAddress = new ApiAddress { Number = "", Flat = "" }, ClientPhone = "", ClientEmail = mail });
            ctx.Services.AddSingleton(container);

            var claims = new List<Claim>();
            claims.Add(new Claim("email", worker_mail));
            ctx.Services.AddSingleton<AuthenticationStateProvider>(new FakeAuthenticationStateProvider(worker_mail, null, claims));

            var httpClient = ctx.Services.AddMockHttpClient();
            httpClient
                .When($"http://localhost:7008/Inquire/{orderCode}/code")
                .RespondJson(new ApiInquire { Source = new ApiAddress { Number = "", Flat = "" }, Destination = new ApiAddress { Number = "", Flat = "" } });

            httpClient
                .When($"http://localhost:7008/Client/{mail}")
                .RespondJson(new ApiClient { Phone = "", Address = new ApiAddress { Number = "", Flat = "" }, SourceAddress = new ApiAddress { Number = "", Flat = "" } });

            httpClient
                .When($"http://localhost:7008/Order/{orderCode}/status")
                .SendJson();

            httpClient
                .When($"http://localhost:7008/OfficeWorker/{worker_mail}/order/{orderCode}/evaluation")
                .SendJson();

            httpClient
                .When($"http://localhost:7008/Content/mailcontent")
                .SendJson();

            // Act
            var component = ctx.RenderComponent<OfficeWorkerReviewOrder>();

            // Assert
            component.WaitForAssertion(() => component.Find(".form-control").TextContent.Equals(name));
            Assert.Equal("Przegląd oferty", component.Find($"h1").TextContent);
            var buttons = component.FindAll(".btn");
            buttons[1].Click();
            buttons[2].Click();
            component.WaitForAssertion(() => component.Find(".p-2").TextContent.Equals("Oferta odrzucona"));
        }
    }
}
