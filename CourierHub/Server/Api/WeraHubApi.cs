using CourierHub.Api.Models.SzymoHubApi;
using CourierHub.Server.Api.Models.WeraHubApi;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Data;
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
        Console.WriteLine("GetOrderStatus was invoked in WeraHubApi.");

        try
        {
            var cancelToken = new CancellationTokenSource(30 * 3000);
            var postResponse = await _httpClient.PostAsync($"/api/Delivery/PostDelivery/{code}", null, cancelToken.Token);
            string? newCode = null;

            if(postResponse != null)
            {
                if(postResponse.StatusCode == HttpStatusCode.UnprocessableEntity)
                {
                    return (StatusType.Denied, StatusCodes.Status200OK, null);
                }

                if(postResponse.StatusCode == HttpStatusCode.Created) 
                {
                    var postResult = await postResponse.Content.ReadFromJsonAsync<WeraRequestAcceptResponse>();
                    if(postResult != null && postResult.result.companyDeliveryId != null) 
                    {
                        code = postResult.result.companyDeliveryId;
                        newCode = code;
                    }
                }
                
                var getResponse = await _httpClient.GetAsync($"/api/Delivery/GetDelivery/{code}", cancelToken.Token);
                if(getResponse != null)
                {
                    switch(getResponse.StatusCode) 
                    {
                        case HttpStatusCode.OK:
                            var getResult = await getResponse.Content.ReadFromJsonAsync<WeraDeliveryResponse>();
                            
                            if(getResult != null && getResult.result != null)
                            {
                                return getResult.result.deliveryStatus switch
                                {
                                    Enums.WeraHubApi.DeliveryStatus.Proccessing => (StatusType.Confirmed, StatusCodes.Status200OK, newCode),
                                    Enums.WeraHubApi.DeliveryStatus.PickedUp => (StatusType.PickedUp, StatusCodes.Status200OK, newCode),
                                    Enums.WeraHubApi.DeliveryStatus.Delivered => (StatusType.Delivered, StatusCodes.Status200OK, newCode),
                                    Enums.WeraHubApi.DeliveryStatus.Canceled => (StatusType.Cancelled, StatusCodes.Status200OK, newCode),
                                    _ => (StatusType.CouldNotDeliver, StatusCodes.Status200OK, newCode)
                                };
                            }
                            break;
                        default:
                            return (StatusType.NotConfirmed, StatusCodes.Status200OK, newCode);
                            
                    }
                }
                
               
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"SzymoHubApi error occurred: {ex.Message}");
        }

        return (null, StatusCodes.Status503ServiceUnavailable, null);
    }

    public async Task<(ApiOffer?, int)> PostInquireGetOffer(ApiInquire inquire) {
        Console.WriteLine("PostInquireGetOffer was invoked in WeronikaHubApi.");
        WeraAddressDto sourceAddress;
        WeraAddressDto destinationAddress;
        WeraPackageDto package;
        WeraInquiryDto weraInquiry;

        Console.WriteLine("[WeraHubApi] Starting creating classes");

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
        catch (Exception e)
        {
            Console.WriteLine($"[WeraHubApi] Error occurred during classes initialization: {e.Message}");
            return (null, 400);
        }
        Console.WriteLine("[WeraGubApi] Sending request");
        var response = new HttpResponseMessage(HttpStatusCode.GatewayTimeout);
        var cancelToken = new CancellationTokenSource(30 * 1000);
        try
        {
            response = await _httpClient.PostAsJsonAsync("/api/Offer/PostOffer", weraInquiry, cancelToken.Token);
        }
        catch (TaskCanceledException e)
        {
            Console.WriteLine("WeraHubApi have not responded within 30 seconds: " + e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("[WeraHubApi]: Error has occurred: " + e.Message);
        }

        Console.WriteLine("[WeraHubApi] Response received");

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
        try
        {
            WeraAddressDto address = new(
            streetName: order.ClientAddress.Street,
            houseNumber: int.Parse(order.ClientAddress.Number),
            flatNumber: order.ClientAddress.Flat == null ? 0 : int.Parse(order.ClientAddress.Flat),
            postcode: order.ClientAddress.PostalCode,
            city: order.ClientAddress.City);

            WeraPersonalDataDto personalData = new(
                name: order.ClientName,
                surname: order.ClientSurname,
                companyName: order.ClientCompany,
                address: address,
                email: order.ClientEmail);

            WeraRequestSendDto request = new(
                companyOfferId: order.Code,
                personalData: personalData);

            var response = new HttpResponseMessage(HttpStatusCode.GatewayTimeout);
            var cancelToken = new CancellationTokenSource(30 * 1000);
            try
            {
                response = await _httpClient.PostAsJsonAsync("/api/Request/PostRequest", request, cancelToken.Token);
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
                var orderResponse = await response.Content.ReadFromJsonAsync<WeraRequestResponseResponse>();
                if (orderResponse != null)
                {
                    return ((int)response.StatusCode, orderResponse.result.companyRequestId);
                }
                else
                {
                    return (StatusCodes.Status503ServiceUnavailable, null);
                }
            }
            return ((int)response.StatusCode, null);
        }
        catch
        {
            return (400, null);

        }



    }

    public async Task<int> PutOrderWithrawal(string code) {
        Console.WriteLine("PutOrderWithdrawal was invoked in WeraHubApi.");

        var response = new HttpResponseMessage(HttpStatusCode.GatewayTimeout);
        var cancelToken = new CancellationTokenSource(30 * 1000);
        try
        {
            response = await _httpClient.DeleteAsync($"api/Delivery/CancelDelivery/{code}", cancelToken.Token);
            if (response != null && response.IsSuccessStatusCode)
                return StatusCodes.Status200OK;
            else
                return StatusCodes.Status403Forbidden;

        }
        catch (TaskCanceledException e)
        {
            Console.WriteLine("WeraHubApi have not responded within 30 seconds: " + e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("[WeraHubApi]: Error has occurred: " + e.Message);
        }
        return StatusCodes.Status503ServiceUnavailable;
    }
}
