using ErrorOr;
using MaxMind.GeoIP2.Exceptions;
using OneOf;
using System.Web;
using CourierHubWebApi.Errors;

namespace CourierHubWebApi.Services.Contracts {
    public interface IPriceCacheService {
        //ErrorOr<DateTime> SavePrice(string InquireCode, decimal Price);
        //ErrorOr<decimal> GetPrice(string InquireCode, DateTime obtainmentTime);
        OneOf<DateTime, ApiError> SavePrice(string InquireCode, decimal Price);
        OneOf<decimal, ApiError> GetPrice(string InquireCode, DateTime obtainmentTime);
    }
}
