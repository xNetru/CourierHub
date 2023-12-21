using CourierHub.Shared.ApiModels;

namespace CourierHub.Shared.Data {
    public class OrderContainer {
        public List<ApiOrder> Inquires { get; set; } = new List<ApiOrder>();
    }
}
