namespace CourierHub.Shared.ApiModels {
    public class ApiReceipt {
        public string Code { get; set; } = null!;

        public DateTime DateTime { get; set; }

        public ApiInquire Inquire { get; set; } = null!;

        public ApiOrder Order { get; set; } = null!;
    }
}
