using CourierHub.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace CourierHub.Client {
    public class Program {
        public static async Task Main(string[] args) {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            var services = builder.Services;
            var configuration = builder.Configuration;

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            services.AddOidcAuthentication(options =>
            {
                configuration.Bind(key: "Auth", options.ProviderOptions);
                options.ProviderOptions.DefaultScopes.Add(item: "email");
            });

            await builder.Build().RunAsync();
        }
    }
}