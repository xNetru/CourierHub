namespace CourierHub.Api.Models.CourierHubApi;

public record ApiSideAddress(
    string City,
    string PostalCode,
    string Street,
    string Number,
    string? Flat);
