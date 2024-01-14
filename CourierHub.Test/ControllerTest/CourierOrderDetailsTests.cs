using Bunit;
using CourierHub.Client.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace CourierHub.Test.ControllerTest {
    public class CourierOrderDetailsTests {
        [Fact]
        public void RendersSuccessfully() {
            using var ctx = new TestContext();

            var mock = ctx.Services.AddMockHttpClient();

            ctx.Services.AddSingleton(new CourierOrderDetails());

            var component = ctx.RenderComponent<CourierOrderDetails>();

            Assert.Equal("Status paczki", component.Find($"h1").TextContent);
        }
    }
}
