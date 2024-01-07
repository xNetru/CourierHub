namespace CourierHubWebApi.Models {
    public record CreateInquireWithEmailRequest(
        CreateInquireRequest standardRequest, string Email);
}
