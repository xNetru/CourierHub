namespace CourierHub.Server.Api.Models.WeraApi
{
    public record WeraPersonalDataDto(
        string? name,
        string? surname,
        string? companyName,
        WeraAddressDto address,
        string? email);

}
