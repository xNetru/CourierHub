namespace CourierHub.Server.Api.Models.WeraApi
{
    public record WeraAddressDto(
        string streetName,
        int houseNumber,
        int flatNumber,
        string postcode,
        string city);
}
