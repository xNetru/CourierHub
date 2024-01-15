namespace CourierHub.IntegrationTest.ApiModels;
public class ApiAddress {
    public string Street { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string? Flat { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string City { get; set; } = null!;
}
