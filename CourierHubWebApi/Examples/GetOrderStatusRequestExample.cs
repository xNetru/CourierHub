using CourierHubWebApi.Models;
using CourierHubWebApi.Validations;
using Swashbuckle.AspNetCore.Filters;

namespace CourierHubWebApi.Examples
{
    public class GetOrderStatusRequestExample : IExamplesProvider<GetOrderStatusRequest>
    {
        GetOrderStatusRequest IExamplesProvider<GetOrderStatusRequest>.GetExamples()
        {
            return new GetOrderStatusRequest("MjAyNG0xbTExNjAwMDAw");
        }
    }
}
