namespace CourierHub.Server.Api.Models.WeraApi
{
    public record WeraRequestResponseDto(
        string? companyRequestId,
        DateTime decisionDeadline);
}
