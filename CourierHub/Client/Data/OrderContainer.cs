using CourierHub.Shared.ApiModels;

namespace CourierHub.Client.Data {
    public class OrderContainer {
        public List<ApiOrder> Orders { get; set; } = new List<ApiOrder>();
    }
}
