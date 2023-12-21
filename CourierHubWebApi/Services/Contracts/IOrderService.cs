using CourierHubWebApi.Models;
using ErrorOr;

namespace CourierHubWebApi.Services.Contracts
{
    public interface IOrderService
    {
        ErrorOr<CreateOrderResponse> CreateOrder(CreateOrderRequest request);
    }
}
