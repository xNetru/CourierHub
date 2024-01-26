using CourierHubWebApi.Errors;
using OneOf;

namespace CourierHubWebApi.Services.Contracts {
    public interface IPriceCacheService {
        OneOf<DateTime, ApiError> SavePrice(string InquireCode, decimal Price);
        OneOf<decimal, ApiError> GetPrice(string InquireCode, DateTime obtainmentTime);
    }
}
