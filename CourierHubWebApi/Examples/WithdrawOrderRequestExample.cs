using CourierHubWebApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection.Metadata.Ecma335;

namespace CourierHubWebApi.Examples
{
    public class WithdrawOrderRequestExample : IExamplesProvider<WithdrawOrderRequest>
    {
        WithdrawOrderRequest IExamplesProvider<WithdrawOrderRequest>.GetExamples()
        {
            return new WithdrawOrderRequest(
                Code: "MjAyNG0xbTExNjAwMDAw");
        }
    }
}
