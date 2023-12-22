namespace CourierHub.CourierHubApiModels;

public record ApiSideAddress(
    string City,
    string PostalCode, 
    string Street,
    string Number,
    string? Flat);
