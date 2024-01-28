namespace CourierHub.Server.Api.Models.WeraApi
{
    public record WeraRequestSendDto(
        string? companyOfferId,
        WeraPersonalDataDto personalData);

}
