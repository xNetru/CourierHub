using Bunit;
using CourierHub.Client.Pages;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Enums;
using RichardSzalay.MockHttp;

namespace CourierHub.Test.FrontendTest
{
    public class CourierOrderDetailsTest
    {
        [Fact]
        public void RendersCorrectly()
        {
            // Arrange
            var orderCode = "123";
            var mail = "test@mail.com";
            var expectedStatusEN = "Confirmed";
            var expectedStatusPL = "Potwierdzone";

            using var ctx = new TestContext();

            var httpClient = ctx.Services.AddMockHttpClient();
            httpClient
                .When($"http://localhost:7008/Order/{orderCode}/status")
                .RespondJson(new ApiStatus { Name = expectedStatusEN });

            httpClient
                .When($"http://localhost:7008/Order/{orderCode}/parcel")
                .RespondJson(new ApiParcel());

            // Act
            var component = ctx.RenderComponent<CourierOrderDetails>(
                parameters => parameters
                    .Add(p => p.orderCode, orderCode)
                    .Add(p => p.mail, mail)
            );

            // Assert
            component.WaitForAssertion(() => component.Find("td:nth-child(2)").TextContent.Equals(expectedStatusPL));
            var tds = component.FindAll("td");
            Assert.Equal("Status paczki", component.Find($"h1").TextContent);
            Assert.Equal(orderCode, tds[0].TextContent);
        }

        [Fact]
        public void PickUpCorrectly()
        {
            // Arrange
            var orderCode = "123";
            var mail = "test@mail.com";
            var expectedStatusEN = "Confirmed";
            var expectedStatusPL = "Potwierdzone";
            var expectedStatusPickedUpPL = "Odebrane";
            var statusType = StatusType.PickedUp;

            using var ctx = new TestContext();

            var httpClient = ctx.Services.AddMockHttpClient();
            httpClient
                .When($"http://localhost:7008/Order/{orderCode}/status")
                .RespondJson(new ApiStatus { Name = expectedStatusEN });

            httpClient
                .When($"http://localhost:7008/Order/{orderCode}/parcel")
                .RespondJson(new ApiParcel());

            httpClient
                .When($"http://localhost:7008/Courier/{mail}/order/{orderCode}/parcel/{(int)statusType}")
                .SendJson();

            // Act
            var component = ctx.RenderComponent<CourierOrderDetails>(
                parameters => parameters
                    .Add(p => p.orderCode, orderCode)
                    .Add(p => p.mail, mail)
            );

            // Assert
            component.WaitForAssertion(() => component.Find("td:nth-child(2)").TextContent.Equals(expectedStatusPL));
            var tds = component.FindAll("td");
            Assert.Equal("Status paczki", component.Find($"h1").TextContent);
            Assert.Equal(orderCode, tds[0].TextContent);
            var button = component.Find(".btn");
            button.Click();
            component.WaitForAssertion(() => component.Find("td:nth-child(2)").TextContent.Equals(expectedStatusPickedUpPL));

        }

        [Fact]
        public void DeliverCorrectly()
        {
            // Arrange
            var orderCode = "123";
            var mail = "test@mail.com";
            var expectedStatusEN = "PickedUp";
            var expectedStatusPL = "Odebrane";
            var expectedStatusDeliveredPL = "Dostarczone";
            var statusType = StatusType.Delivered;

            using var ctx = new TestContext();

            var httpClient = ctx.Services.AddMockHttpClient();
            httpClient
                .When($"http://localhost:7008/Order/{orderCode}/status")
                .RespondJson(new ApiStatus { Name = expectedStatusEN });

            httpClient
                .When($"http://localhost:7008/Order/{orderCode}/parcel")
                .RespondJson(new ApiParcel());

            httpClient
                .When($"http://localhost:7008/Courier/{mail}/order/{orderCode}/parcel/{(int)statusType}")
                .SendJson();

            // Act
            var component = ctx.RenderComponent<CourierOrderDetails>(
                parameters => parameters
                    .Add(p => p.orderCode, orderCode)
                    .Add(p => p.mail, mail)
            );

            // Assert
            component.WaitForAssertion(() => component.Find("td:nth-child(2)").TextContent.Equals(expectedStatusPL));
            var tds = component.FindAll("td");
            Assert.Equal("Status paczki", component.Find($"h1").TextContent);
            Assert.Equal(orderCode, tds[0].TextContent);
            var buttons = component.FindAll(".btn");
            buttons[1].Click();
            component.WaitForAssertion(() => component.Find("td:nth-child(2)").TextContent.Equals(expectedStatusDeliveredPL));
        }

        [Fact]
        public void NotDeliveredCorrectly()
        {
            // Arrange
            var orderCode = "123";
            var mail = "test@mail.com";
            var expectedStatusEN = "PickedUp";
            var expectedStatusPL = "Odebrane";
            var expectedStatusNotDeliveredPL = "Nie dostarczone";
            var statusType = StatusType.CouldNotDeliver;

            using var ctx = new TestContext();

            var httpClient = ctx.Services.AddMockHttpClient();
            httpClient
                .When($"http://localhost:7008/Order/{orderCode}/status")
                .RespondJson(new ApiStatus { Name = expectedStatusEN });

            httpClient
                .When($"http://localhost:7008/Order/{orderCode}/parcel")
                .RespondJson(new ApiParcel());

            httpClient
                .When($"http://localhost:7008/Courier/{mail}/order/{orderCode}/parcel/{(int)statusType}")
                .SendJson();

            // Act
            var component = ctx.RenderComponent<CourierOrderDetails>(
                parameters => parameters
                    .Add(p => p.orderCode, orderCode)
                    .Add(p => p.mail, mail)
            );

            // Assert
            component.WaitForAssertion(() => component.Find("td:nth-child(2)").TextContent.Equals(expectedStatusPL));
            var tds = component.FindAll("td");
            Assert.Equal("Status paczki", component.Find($"h1").TextContent);
            Assert.Equal(orderCode, tds[0].TextContent);
            var buttons = component.FindAll(".btn");
            buttons[2].Click();
            buttons[3].Click();
            component.WaitForAssertion(() => component.Find("td:nth-child(2)").TextContent.Equals(expectedStatusNotDeliveredPL));
        }

    }
}
