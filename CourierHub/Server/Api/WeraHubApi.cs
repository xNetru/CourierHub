using CourierHub.Server.Api.Models.WeraApi;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Enums;

namespace CourierHub.Server.Api;
public class WeraHubApi : IWebApi {
    private readonly ApiService _service;
    private readonly HttpClient _httpClient;
    public string ServiceName { get; set; }

    public WeraHubApi(ApiService service) {
        _service = service;
        ServiceName = _service.Name;
    }
    public async Task<(StatusType?, int, string?)> GetOrderStatus(string code) {
        Console.WriteLine("GetOrderStatus was invoked in WeronikaHubApi.");
        return (null, 400, null);
    }

    public async Task<(ApiOffer?, int)> PostInquireGetOffer(ApiInquire inquire) {
        WeraAddressDto sourceAddress;
        WeraAddressDto destinationAddress;
        WeraPackageDto package;
        WeraInquiryDto inquiry;

        try
        {
            sourceAddress = new WeraAddressDto(
                streetName: inquire.Source.Street,
                houseNumber: int.Parse(inquire.Source.Number),
                flatNumber: inquire.Source.Flat == null ? 0 : int.Parse(inquire.Source.Flat),
                postcode: inquire.Source.PostalCode,
                city: inquire.Source.City);

            destinationAddress = new WeraAddressDto(
                streetName: inquire.Destination.Street,
                houseNumber: int.Parse(inquire.Destination.Number),
                flatNumber: inquire.Destination.Flat == null ? 0 : int.Parse(inquire.Destination.Flat),
                postcode: inquire.Destination.PostalCode,
                city: inquire.Destination.City);

            package = new WeraPackageDto(
                width: inquire.Width / 100.0f,
                height: inquire.Depth / 100.0f,
                length: inquire.Length / 100.0f,
                dimensionsUnit: "Meters",
                weight: inquire.Mass / 1000.0f,
                weightUnit: "Kilograms");

            inquiry = new WeraInquiryDto(
                pickupDate: inquire.SourceDate,
                deliveryDate: inquire.DestinationDate,
                IsPriority: inquire.Priority != (int)PriorityType.Low,
                sourceAddress: sourceAddress,
                destinationAddress: destinationAddress,
                package: package);
        } 
        catch 
        {
            return (null, 400);
        }


        Console.WriteLine("PostInquireGetOffer was invoked in WeronikaHubApi.");
        return (null, 400);
    }

    public async Task<(int, string?)> PostOrder(ApiOrder order) {
        Console.WriteLine("PostOrder was invoked in WeronikaHubApi.");
        return (400, null);
    }

    public async Task<int> PutOrderWithrawal(string code) {
        Console.WriteLine("PutOrderWithrawal was invoked in WeronikaHubApi.");
        return 400;
    }
}
