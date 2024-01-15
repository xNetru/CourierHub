﻿using CourierHubWebApi.Services.Contracts;
using ErrorOr;
using OneOf;

namespace CourierHubWebApi.Services {
    public class PriceCacheService : IPriceCacheService {
        private static Dictionary<string, OfferData> _registeredOffers = new Dictionary<string, OfferData>();

        public OneOf<DateTime, int> SavePrice(string InquireCode, decimal Price)
        {
            DateTime timeStamp = DateTime.Now.AddMinutes(15);
            if (!_registeredOffers.TryAdd(InquireCode,
                                new OfferData(Price, timeStamp)))
            {
                return StatusCodes.Status403Forbidden;
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

        public OneOf<decimal, int> GetPrice(string InquireCode, DateTime obtainmentTime)
        {
            if (!_registeredOffers.TryGetValue(InquireCode, out OfferData? offer))
            {
                return StatusCodes.Status404NotFound;
            }
            if (obtainmentTime > offer.TimeStamp)
            {
                return StatusCodes.Status408RequestTimeout;
            }
            try
            {
                _registeredOffers.Remove(InquireCode);
                return offer.Price;
            }
            catch
            {
                return StatusCodes.Status500InternalServerError;
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
