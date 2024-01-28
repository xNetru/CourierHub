namespace CourierHub.Api.Models.SzymoHubApi;

public record SzymoPostOfferRequest(
    string inquiryId,
    string name,
    string email,
    SzymoAddress address);
