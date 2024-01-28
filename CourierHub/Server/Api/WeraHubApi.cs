using CourierHub.Api.Models.SzymoHubApi;
using CourierHub.Server.Api.Models.WeraApi;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Enums;
using System.Net;

namespace CourierHub.Server.Api;
public class WeraHubApi : IWebApi {
    private readonly HttpClient _httpClient = new();
    public string ServiceName { get; set; }

    public WeraHubApi(ApiService service) {
        ServiceName = service.Name;
        _httpClient.BaseAddress = new Uri(service.BaseAddress);
        _httpClient.DefaultRequestHeaders.Add("x-api-key", service.ApiKey);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }
    public async Task<(StatusType?, int, string?)> GetOrderStatus(string code) {
        Console.WriteLine("GetOrderStatus was invoked in WeronikaHubApi.");
        return (null, 400, null);
    }

    public async Task<(ApiOffer?, int)> PostInquireGetOffer(ApiInquire inquire) {
        WeraAddressDto sourceAddress;
        WeraAddressDto destinationAddress;
        WeraPackageDto package;
        WeraInquiryDto weraInquiry;

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

            weraInquiry = new WeraInquiryDto(
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

        var response = new HttpResponseMessage(HttpStatusCode.GatewayTimeout);
        var cancelToken = new CancellationTokenSource(30 * 1000);
        try
        {
            response = await _httpClient.PostAsJsonAsync("/Inquires", weraInquiry, cancelToken.Token);
        }
        catch (TaskCanceledException e)
        {
            Console.WriteLine("WeraHubApi have not responded within 30 seconds: " + e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("[WeraHubApi]: Error has occurred: " + e.Message);
        }

        if (response.IsSuccessStatusCode)
        {
            var inquireResponse = await response.Content.ReadFromJsonAsync<WeraOfferResponse>();
            if (inquireResponse != null)
            {
                var offer = new ApiOffer
                {
                    Price = (decimal)inquireResponse.result.price,
                    Code = inquireResponse.result.companyOfferId!,
                    ServiceName = ServiceName,
                    ExpirationDate = inquireResponse.result.expirationDate
                };
                return (offer, (int)response.StatusCode);
            }
            else
            {
                return (null, StatusCodes.Status503ServiceUnavailable);
            }
        }
        else
        {
            return (null, (int)response.StatusCode);
        }
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
