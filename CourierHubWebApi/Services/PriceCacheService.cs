using CourierHubWebApi.Services.Contracts;
using OneOf;
using CourierHubWebApi.Errors;

namespace CourierHubWebApi.Services {
    public class PriceCacheService : IPriceCacheService {
        private static Dictionary<string, OfferData> _registeredOffers = new Dictionary<string, OfferData>();

        public OneOf<DateTime, ApiError> SavePrice(string InquireCode, decimal Price)
        {
            DateTime timeStamp = DateTime.Now.AddMinutes(15);
            if (!_registeredOffers.TryAdd(InquireCode,
                                new OfferData(Price, timeStamp)))
            {
                return ApiError.DefaultInternalServerError;
            }
            return timeStamp;
        }

        public OneOf<decimal, ApiError> GetPrice(string InquireCode, DateTime obtainmentTime)
        {
            if (!_registeredOffers.TryGetValue(InquireCode, out OfferData? offer))
            {
                return new ApiError(StatusCodes.Status404NotFound, "Such offer does not exist.", "Offer not found.");
            }
            if (obtainmentTime > offer.TimeStamp)
            {
                return new ApiError(StatusCodes.Status408RequestTimeout, "Offer expired.", "Request timeout");
            }
            try
            {
                _registeredOffers.Remove(InquireCode);
                return offer.Price;
            }
            catch
            {
                return ApiError.DefaultInternalServerError;
            }
        }
        private record OfferData(decimal Price,
                                 DateTime TimeStamp);
    }
}
