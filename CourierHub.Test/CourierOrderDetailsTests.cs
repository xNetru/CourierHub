using CourierHub.Client.Pages;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using CourierHub.Shared.Models;

namespace CourierHub.Test
{
    public class CourierOrderDetailsTests
    {
        [Fact]
        public void RendersSuccessfully()
        {
            using var ctx = new TestContext();

            var mock = ctx.Services.AddMockHttpClient();

            ctx.Services.AddSingleton<CourierOrderDetails>(new CourierOrderDetails());

            var component = ctx.RenderComponent<CourierOrderDetails>();

            Assert.Equal("Status paczki", component.Find($"h1").TextContent);
        }
    }
}
