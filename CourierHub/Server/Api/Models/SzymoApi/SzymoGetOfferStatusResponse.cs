namespace CourierHub.Api.Models.SzymoApi;

public record SzymoGetOfferStatusResponse(
    string offerId,
    bool isReady,
    DateTime timestamp);
