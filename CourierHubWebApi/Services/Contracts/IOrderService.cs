using CourierHubWebApi.Models;
using ErrorOr;

namespace CourierHubWebApi.Services.Contracts {
    public interface IOrderService {
        ErrorOr<int> CreateOrder(CreateOrderRequest request, int serviceId);
    }
}
