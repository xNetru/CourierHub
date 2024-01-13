using CourierHub.Server.Api;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;

namespace CourierHub.Server.Containers;
public class WebApiContainer {
    public List<IWebApi> WebApis { get; set; }

    public WebApiContainer(CourierHubDbContext context) {
        var services = context.Services.ToList();
        WebApis = new List<IWebApi>();
        foreach (var service in services) {
            switch (service.Name) {
                case "CourierHub": {
                    WebApis.Add(new CourierHubApi((ApiService)service));
                    break;
                }
                case "MiNI.Courier.API": {
                    WebApis.Add(new SzymoHubApi((ApiService)service, new AccessTokenContainer()));
                    break;
                }
            }
        }
    }
}
