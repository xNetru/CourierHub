namespace CourierHub.CourierHubApiModels;

public record CreateInquireWithEmailRequest(CreateInquireRequest standardRequest, string Email);

