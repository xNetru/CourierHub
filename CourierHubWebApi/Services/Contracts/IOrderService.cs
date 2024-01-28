using CourierHub.Shared.Enums;
using CourierHubWebApi.Errors;
using CourierHubWebApi.Models;
using OneOf;

namespace CourierHubWebApi.Services.Contracts {
    public interface IOrderService {
        OneOf<int, ApiError> CreateOrder(CreateOrderRequest request, int serviceId);
        Task<OneOf<int, ApiError>> WithdrawOrder(WithdrawOrderRequest request, int serviceId);
        OneOf<StatusType, ApiError> GetOrderStatus(GetOrderStatusRequest request, int serviceId);
    }
}
