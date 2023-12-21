namespace CourierHub.Shared.ApiModels {
    public class ApiMailContent {
        public string Recipient { get; set; } = null!;

        public string Link { get; set; } = null!;

        public ApiClient Client { get; set; } = null!;

        public ApiContract? Contract { get; set; }

        public ApiReceipt? Receipt { get; set; }
    }
}
