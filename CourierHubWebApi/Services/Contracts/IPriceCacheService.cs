using ErrorOr;
using OneOf;

namespace CourierHubWebApi.Services.Contracts {
    public interface IPriceCacheService {
        //ErrorOr<DateTime> SavePrice(string InquireCode, decimal Price);
        //ErrorOr<decimal> GetPrice(string InquireCode, DateTime obtainmentTime);
        OneOf<DateTime, int> SavePrice(string InquireCode, decimal Price);
        OneOf<decimal, int> GetPrice(string InquireCode, DateTime obtainmentTime);
    }
}
