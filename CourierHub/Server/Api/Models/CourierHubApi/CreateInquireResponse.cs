namespace CourierHub.Api.Models.CourierHubApi;

public record CreateInquireResponse(
    decimal Price,
    string Code,
    DateTime ExpirationDate);
