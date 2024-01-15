using Bunit;
using CourierHub.Client.Pages;
using CourierHub.Shared.ApiModels;
using RichardSzalay.MockHttp;

namespace CourierHub.Test.FrontendTest
{
    public class OrderStatusTest
    {
        [Fact]
        public void RendersCorrectly()
        {
            // Arrange
            var orderCode = "123";
            var service = "CourierHub";
            var expectedStatusEN = "Confirmed";
            var expectedStatusPL = "Potwierdzone";

            using var ctx = new TestContext();

            var httpClient = ctx.Services.AddMockHttpClient();
            httpClient
                .When($"http://localhost:7008/Order/{orderCode}/status")
                .RespondJson(new ApiStatus { Name = expectedStatusEN });

            httpClient
                .When($"http://localhost:7008/Order/{orderCode}/service")
                .RespondString($"<title>{service}</title>");

            httpClient
                .When($"http://localhost:7008/Order/{orderCode}/evaluation")
                .RespondJson(new ApiEvaluation());

            httpClient
                .When($"http://localhost:7008/Order/{orderCode}/review")
                .RespondJson(new ApiReview());

            httpClient
                .When($"http://localhost:7008/Order/{orderCode}/parcel")
                .RespondJson(new ApiParcel());

            // Act
            var component = ctx.RenderComponent<OrderStatus>(
                parameters => parameters
                    .Add(p => p.orderCode, orderCode)
            );

            // Assert
            component.WaitForAssertion(() => component.Find("td:nth-child(1)").TextContent.Equals(service));
            component.WaitForAssertion(() => component.Find("td:nth-child(2)").TextContent.Equals(orderCode));
            component.WaitForAssertion(() => component.Find("td:nth-child(3)").TextContent.Equals(expectedStatusPL));
            Assert.Equal("Status zamówienia", component.Find($"h1").TextContent);
        }

    }
}
