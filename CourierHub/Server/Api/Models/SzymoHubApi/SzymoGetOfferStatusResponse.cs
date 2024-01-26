namespace CourierHub.Api.Models.SzymoHubApi;

public record SzymoGetOfferStatusResponse(
    string offerId,
    bool isReady,
    DateTime timestamp);
