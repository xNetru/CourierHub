using CourierHub.Shared.Enums;

namespace CourierHub.Api.Models.CourierHubApi;

public record CreateInquireRequest(
    int Depth,
    int Width,
    int Length,
    int Mass,
    ApiSideAddress SourceAddress,
    ApiSideAddress DestinationAddress,
    DateTime SourceDate,
    DateTime DestinationDate,
    DateTime Datetime,
    bool IsCompany,
    bool IsWeekend,
    PriorityType Priority);
