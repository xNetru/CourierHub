using Bunit;
using CourierHub.Client.Data;
using CourierHub.Client.Pages;
using CourierHub.Shared.ApiModels;
using Microsoft.Extensions.DependencyInjection;
using RichardSzalay.MockHttp;

namespace CourierHub.Test.FrontendTest {
    public class OfferSubmitFormTest {
        [Fact]
        public void RendersCorrectly() {
            // Arrange
            var mail = "test@mail.com";
            var orderCode = "123";
            var service = "CourierHub";

            using var ctx = new TestContext();

            RoleContainer role_container = new RoleContainer();
            role_container.Roles["Client"] = true;
            role_container.Roles["NotAuthorized"] = false;
            ctx.Services.AddSingleton(role_container);

            OfferContainer offer_container = new OfferContainer();
            offer_container.Offers.Add(new ApiOffer { Code = orderCode, ServiceName = service });
            ctx.Services.AddSingleton(offer_container);

            InquireContainer inquire_container = new InquireContainer();
            inquire_container.Inquires.Add(new ApiInquire { Code = orderCode, Destination = new ApiAddress { Number = "", Flat = "" }, Source = new ApiAddress { Number = "", Flat = "" } });
            ctx.Services.AddSingleton(inquire_container);

            var httpClient = ctx.Services.AddMockHttpClient();
            httpClient
                .When($"http://localhost:7008/Client/{mail}")
                .RespondJson(new ApiClient { Email = mail, Name = mail, Phone = "", Address = new ApiAddress { Flat = "", Number = "" }, SourceAddress = new ApiAddress { Flat = "", Number = "" } });

            // Act
            var component = ctx.RenderComponent<OfferSubmitForm>(
                parameters => parameters
                    .Add(p => p.mail, mail)
            );

            // Assert
            component.WaitForAssertion(() => component.Find(".form-control").TextContent.Equals(mail));
            Assert.Equal("Dane osobowe", component.Find($"h1").TextContent);
        }

    }
}
