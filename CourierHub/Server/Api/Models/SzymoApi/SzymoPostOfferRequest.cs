namespace CourierHub.Api.Models.SzymonApi;

public record SzymoPostOfferRequest(
    string inquiryId,
    string name,
    string email,
    SzymoAddress address);
