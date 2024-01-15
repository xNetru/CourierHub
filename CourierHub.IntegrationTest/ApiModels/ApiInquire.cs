namespace CourierHub.IntegrationTest.ApiModels;

public class ApiInquire {
    public string? Email { get; set; }

    public int Depth { get; set; }

    public int Width { get; set; }

    public int Length { get; set; }

    public int Mass { get; set; }

    public DateTime SourceDate { get; set; } = DateTime.Today;

    public DateTime DestinationDate { get; set; } = DateTime.Today;

    public DateTime Datetime { get; set; }

    public bool IsCompany { get; set; }

    public bool IsWeekend { get; set; }

    public int Priority { get; set; } = -1;

    public string Code { get; set; } = null!;

    public ApiAddress Destination { get; set; } = null!;

    public ApiAddress Source { get; set; } = null!;
}
