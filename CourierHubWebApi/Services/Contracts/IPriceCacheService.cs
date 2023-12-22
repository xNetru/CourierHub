using ErrorOr;

namespace CourierHubWebApi.Services.Contracts {
    public interface IPriceCacheService {
        ErrorOr<DateTime> SavePrice(string InquireCode, decimal Price);
        ErrorOr<decimal> GetPrice(string InquireCode, DateTime obtainmentTime);
    }
}
