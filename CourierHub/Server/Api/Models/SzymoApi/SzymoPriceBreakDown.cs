namespace CourierHub.Api.Models.SzymoApi;

public record SzymoPriceBreakDown(
    double amount,
    string? currency,
    string? description);
