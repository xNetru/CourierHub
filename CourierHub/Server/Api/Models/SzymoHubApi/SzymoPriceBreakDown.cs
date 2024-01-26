namespace CourierHub.Api.Models.SzymoHubApi;

public record SzymoPriceBreakDown(
    double amount,
    string? currency,
    string? description);
