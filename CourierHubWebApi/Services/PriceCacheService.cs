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
                return new ApiError(StatusCodes.Status500InternalServerError, "Cannot prepare offer", "Internal server error");
            }
            return timeStamp;
        }
        //public ErrorOr<DateTime> SavePrice(string InquireCode, decimal Price) {
        //    DateTime timeStamp = DateTime.Now.AddMinutes(15);
        //    if (!_registeredOffers.TryAdd(InquireCode,
        //                        new OfferData(Price, timeStamp))) {
        //        return Error.Conflict();
        //    }
        //    return timeStamp;
        //}

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
                return new ApiError(StatusCodes.Status500InternalServerError, null, "Internal server error.";
            }
        }

        //public ErrorOr<decimal> GetPrice(string InquireCode, DateTime obtainmentTime) {
        //    if (!_registeredOffers.TryGetValue(InquireCode, out OfferData? offer)) {
        //        return Error.NotFound();
        //    }
        //    if (obtainmentTime > offer.TimeStamp) {
        //        return Error.Custom(type: 408, code: "Request Timeout", description: "Offer expired");
        //    }
        //    try {
        //        _registeredOffers.Remove(InquireCode);
        //        return offer.Price;
        //    } catch (Exception ex) {
        //        return Error.Unexpected();
        //    }
        //}
        private record OfferData(decimal Price,
                                 DateTime TimeStamp);
    }
}
