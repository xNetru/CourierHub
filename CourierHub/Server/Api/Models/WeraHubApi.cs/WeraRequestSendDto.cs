namespace CourierHub.Server.Api.Models.WeraHubApi
{
    public record WeraRequestSendDto(
        string? companyOfferId,
        WeraPersonalDataDto personalData);

}
