namespace CourierHub.IntegrationTest.ApiModels;

public class ApiOffer {
    public decimal Price { get; set; }

    public string Code { get; set; } = null!;

    public string ServiceName { get; set; } = null!;

    public DateTime ExpirationDate { get; set; }
}
