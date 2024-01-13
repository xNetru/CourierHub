namespace CourierHub.Api.Models.SzymonApi;

public record SzymoPriceBreakDown(
    double amount,
    string? currency,
    string? description);
