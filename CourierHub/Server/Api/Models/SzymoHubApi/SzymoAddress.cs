namespace CourierHub.Api.Models.SzymoHubApi;

public record SzymoAddress(
    string? houseNumber,
    string? apartmentNumber,
    string? street,
    string? city,
    string? zipCode,
    string? country);
