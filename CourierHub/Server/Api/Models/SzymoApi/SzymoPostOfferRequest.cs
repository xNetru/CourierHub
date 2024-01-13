namespace CourierHub.Api.Models.SzymoApi;

public record SzymoPostOfferRequest(
    string inquiryId,
    string name,
    string email,
    SzymoAddress address);
