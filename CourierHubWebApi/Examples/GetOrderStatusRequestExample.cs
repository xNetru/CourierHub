using CourierHubWebApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace CourierHubWebApi.Examples {
    public class GetOrderStatusRequestExample : IExamplesProvider<GetOrderStatusRequest> {
        GetOrderStatusRequest IExamplesProvider<GetOrderStatusRequest>.GetExamples() {
            return new GetOrderStatusRequest("MjAyNG0xbTExNjAwMDAw");
        }
    }
}
