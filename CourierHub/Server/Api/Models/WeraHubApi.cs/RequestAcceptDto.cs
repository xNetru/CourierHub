using CourierHub.Server.Api.Enums.WeraHubApi;

namespace CourierHub.Server.Api.Models.WeraHubApi
{
    public record RequestAcceptDto(
        string? companyDeliveryId,
        WeraRequestStatus requestStatus);
}
