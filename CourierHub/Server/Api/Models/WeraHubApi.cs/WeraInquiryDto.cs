namespace CourierHub.Server.Api.Models.WeraHubApi
{
    public record WeraInquiryDto(
        DateTime pickupDate,
        DateTime deliveryDate,
        bool IsPriority, 
        WeraAddressDto sourceAddress,
        WeraAddressDto destinationAddress,
        WeraPackageDto package);
}
