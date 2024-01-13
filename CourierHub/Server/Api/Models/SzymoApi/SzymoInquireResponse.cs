namespace CourierHub.Api.Models.SzymonApi;

public record SzymoInquireResponse(
    string inquiryId,
    double totalPrice,
    string? currency,
    DateTime expiringAt,
    SzymoPriceBreakDown? priceBreakDown);
