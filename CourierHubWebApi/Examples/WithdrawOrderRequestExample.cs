using CourierHubWebApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace CourierHubWebApi.Examples {
    public class WithdrawOrderRequestExample : IExamplesProvider<WithdrawOrderRequest> {
        WithdrawOrderRequest IExamplesProvider<WithdrawOrderRequest>.GetExamples() {
            return new WithdrawOrderRequest(
                Code: "MjAyNG0xbTExNjAwMDAw");
        }
    }
}
