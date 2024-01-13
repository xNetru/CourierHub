namespace CourierHub.Api.Models.SzymonApi;

public record SzymoGetOfferStatusResponse(
    string offerId,
    bool isReady,
    DateTime timestamp);
