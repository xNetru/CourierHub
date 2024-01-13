namespace CourierHub.Api.Models.CourierHubApi;

public record CreateOrderRequest(
    string InquireCode,
    string ClientName,
    string ClientSurname,
    string ClientEmail,
    string ClientPhoneNumber,
    string ClientCompany,
    ApiSideAddress ClientAddress);

