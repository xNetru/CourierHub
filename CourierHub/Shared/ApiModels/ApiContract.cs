namespace CourierHub.Shared.ApiModels {
    public class ApiContract {
        public string Code { get; set; } = null!;

        public DateTime DateTime { get; set; }

        public ApiClient Client { get; set; } = null!;
    }
}
