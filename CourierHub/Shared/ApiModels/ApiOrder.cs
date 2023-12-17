namespace CourierHub.Shared.ApiModels {
    public class ApiOrder {
        public int ApiInquire { get; set; }

        public decimal Price { get; set; }

        public string ClientEmail { get; set; } = null!;

        public string ClientName { get; set; } = null!;

        public string ClientSurname { get; set; } = null!;

        public string ClientPhone { get; set; } = null!;

        public string ClientCompany { get; set; } = null!;

        public ApiAddress ClientAddress { get; set; } = null!;
    }
}
