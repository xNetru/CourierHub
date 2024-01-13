namespace CourierHub.Api.Models.SzymonApi;

public record SzymoAddress(
    string? houseNumber,
    string? apartmentNumber,
    string? street,
    string? city,
    string? zipCode,
    string? country);
