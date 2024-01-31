namespace CourierHub.Server.Api.Models.WeraHubApi
{
    public record WeraRequestResponseDto(
        string? companyRequestId,
        DateTime decisionDeadline);
}
