namespace CourierHubWebApi.Models {
    public record CreateInquireResponse(
        decimal Price,
        string Code,
        DateTime ExpirationDate);
}
