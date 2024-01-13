using CourierHub.Shared.ApiModels;

namespace CourierHub.Client.Data {
    public class OfferContainer {
        public List<ApiOffer> Offers { get; set; } = new List<ApiOffer>();
    }
}