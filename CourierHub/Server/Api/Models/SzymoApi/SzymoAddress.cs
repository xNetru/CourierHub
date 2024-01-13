namespace CourierHub.Api.Models.SzymoApi;

public record SzymoAddress(
    string? houseNumber,
    string? apartmentNumber,
    string? street,
    string? city,
    string? zipCode,
    string? country);
