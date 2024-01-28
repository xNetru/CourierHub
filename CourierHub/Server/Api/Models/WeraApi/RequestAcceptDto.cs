using CourierHub.Server.Api.Enums.WeraApi;

namespace CourierHub.Server.Api.Models.WeraApi
{
    public record RequestAcceptDto(
        string? companyDeliveryId,
        WeraRequestStatus requestStatus);
}
