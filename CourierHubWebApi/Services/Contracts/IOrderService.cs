using CourierHub.Shared.Enums;
using CourierHubWebApi.Models;
using ErrorOr;
using OneOf;
using Microsoft.AspNetCore.Http;
using CourierHubWebApi.Errors;

namespace CourierHubWebApi.Services.Contracts {
    public interface IOrderService {
        OneOf<int, ApiError> CreateOrder(CreateOrderRequest request, int serviceId);
        Task<OneOf<int, ApiError>> WithdrawOrder(WithdrawOrderRequest request, int serviceId);
        OneOf<StatusType, ApiError> GetOrderStatus(GetOrderStatusRequest request, int serviceId);
    }
}
