using CourierHub.Shared.ApiModels;

namespace CourierHub.Shared.Data {
    public class OfferContainer {
        public List<ApiOffer> Offers { get; set; } = new List<ApiOffer>();
    }
}