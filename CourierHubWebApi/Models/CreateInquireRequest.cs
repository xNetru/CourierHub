using CourierHub.Shared.Enums;

namespace CourierHubWebApi.Models {
    /// <summary>
    /// Inquiry
    /// </summary>
    /// <param name="Depth" example="2"></param>
    /// <param name="Width"></param>
    /// <param name="Length"></param>
    /// <param name="Mass"></param>
    /// <param name="SourceAddress"></param>
    /// <param name="DestinationAddress"></param>
    /// <param name="SourceDate"></param>
    /// <param name="DestinationDate"></param>
    /// <param name="Datetime"></param>
    /// <param name="IsCompany"></param>
    /// <param name="IsWeekend"></param>
    /// <param name="Priority"></param>
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
}
