using CourierHub.Shared.Enums;
using CourierHubWebApi.Models;
using ErrorOr;

namespace CourierHubWebApi.Services.Contracts {
    public interface IOrderService {
        ErrorOr<int> CreateOrder(CreateOrderRequest request, int serviceId);
        Task<ErrorOr<int>> WithdrawOrder(WithdrawOrderRequest request, int serviceId);
        ErrorOr<StatusType> GetOrderStatus(GetOrderStatusRequest request, int serviceId);
    }
}
