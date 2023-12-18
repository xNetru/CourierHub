namespace CourierHubWebApi.Models {
    public record CreateInquireRequest(
        // int? ClientId,
        int Depth,
        int Width,
        int Length,
        int Mass,
        string SourceCity,
        string SourceStreet,
        string SourceNumber,
        string SourceFlat,
        string SourcePostalCode,
        string DestinationCity,
        string DestinationStreet,
        string DestinationNumber,
        string DestinationFlat,
        string DestinationPostalCode,
        DateTime SourceDate,
        DateTime DestinationDate,
        DateTime Datetime,
        bool IsCompany,
        bool IsWeekend,
        int Priority);
}
