using CourierHub.Shared.ApiModels;

namespace CourierHub.Client.Data {
    public class InquireContainer {
        public List<ApiInquire> Inquires { get; set; } = new List<ApiInquire>();
    }
}
