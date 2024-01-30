namespace CourierHub.Server.Api.Models.WeraHubApi
{
    public record WeraAddressDto(
        string streetName,
        int houseNumber,
        int flatNumber,
        string postcode,
        string city);
}
