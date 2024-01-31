namespace CourierHub.Server.Api.Models.WeraHubApi
{
    public record WeraPersonalDataDto(
        string? name,
        string? surname,
        string? companyName,
        WeraAddressDto address,
        string? email);

}
