namespace CourierHub.CourierHubApiModels;

public record CreateInquireResponse(
    decimal Price,
    string Code,
    DateTime ExpirationDate);
