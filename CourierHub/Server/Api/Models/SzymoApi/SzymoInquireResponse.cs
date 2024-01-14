namespace CourierHub.Api.Models.SzymoApi;

public record SzymoInquireResponse(
    string inquiryId,
    double totalPrice,
    string? currency,
    DateTime expiringAt);//,
    //SzymoPriceBreakDown? priceBreakDown);
