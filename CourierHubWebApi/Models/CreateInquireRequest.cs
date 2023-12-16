using System.ComponentModel.DataAnnotations;


namespace CourierHubWebApi.Models
{
    public record CreateInquireRequest(
        int? ClientId,
        [Required]
        int Depth,
        [Required]
        int Width,
        [Required]
        int Length,
        [Required]
        int Mass,
        [Required]
        string SourceStreet,
        [Required]
        string SourceNumber,
        [Required]
        string SourceFlat,
        [Required]
        string SourcePostalCode,
        [Required]
        string DestinationStreet,
        [Required]
        string DestinationNumber,
        [Required]
        string DestinationFlat,
        [Required]
        string DestinationPostalCode,
        [Required]
        DateTime SourceDate,
        [Required]
        DateTime DestinationDate,
        [Required]
        DateTime Datetime,
        [Required]
        bool IsCompany,
        [Required]
        bool IsWeekend,
        [Required]
        int Priority);
}
