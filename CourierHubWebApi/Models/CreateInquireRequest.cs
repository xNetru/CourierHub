namespace CourierHubWebApi.Models {
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
        int Priority);
}
