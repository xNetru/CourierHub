using CourierHub.CourierHubApiModels;
using CourierHub.Shared.Abstractions;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Enums;
using CourierHub.Shared.SzymoApiModels;
using CourierHub.Shared.Data;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using System.Net.Http;
using System.Net;
using CourierHubWebApi.Models;

namespace CourierHub.Server.Data;
public class SzymoHubApi : IWebApi {
    private readonly ApiService _service;
    private readonly HttpClient _httpClient = new();
    private readonly AccessTokenContainer _accessTokenContainer;
    private static string _tokenEndPoint = "/connect/token";
    public string ServiceName { get; set; }

    public SzymoHubApi(ApiService service, IConfiguration config, AccessTokenContainer accessTokenContainer) {
        _service = service;
        _service.BaseAddress = config.GetValue<string>("SzymoAddress") ??
            throw new NullReferenceException("Base address could not be loaded!");
        ServiceName = _service.Name;
        _httpClient.BaseAddress = new Uri(_service.BaseAddress);
        _accessTokenContainer = accessTokenContainer;
    }

    public async Task<(StatusType?, int)> GetOrderStatus(string code) {
        Console.WriteLine("GetOrderStatus was invoked in SzymoHubApi.");

        StatusType? status = null;
        var cancelToken = new CancellationTokenSource(30 * 1000);
        try
        {
            status = await _httpClient.GetFromJsonAsync<StatusType?>($"/offer/request/{code}/status", cancelToken.Token);
        }
        catch (TaskCanceledException e)
        {
            Console.WriteLine("CourierHubApi have not responded within 30 seconds: " + e.Message);
        }

        if (status == null)
        {
            return (null, 504);
        }
        else
        {
            return (status, 200);
        }
    }

    public async Task<(ApiOffer?, int)> PostInquireGetOffer(ApiInquire inquire) {
        Console.WriteLine("PostInquireGetOffer was invoked in SzymoHubApi.");

        var source = new SzymoAddress(
            inquire.Source.Number,
            inquire.Source.Flat,
            inquire.Source.Street,
            inquire.Source.City,
            inquire.Source.PostalCode,
            "Poland");

        var destination = new SzymoAddress(
            inquire.Destination.Number,
            inquire.Destination.Flat,
            inquire.Destination.Street,
            inquire.Destination.City,
            inquire.Destination.PostalCode,
            "Poland");

        var dimensions = new SzymoDimensions(
            inquire.Width / 100.0f,
            inquire.Length / 100.0f,
            inquire.Depth / 100.0f,
            "Meters");

        var szymoInquiry = new SzymoInquiry(
            dimensions,
            "Pln",
            inquire.Mass / 1000.0f,
            "Kilograms",
            source,
            destination,
            inquire.SourceDate,
            inquire.DestinationDate,
            inquire.IsWeekend,
            inquire.Priority switch
            {
                (int)PriorityType.High => "High",
                (int)PriorityType.Low => "Low",
                (int)PriorityType.Medium => "Medium",
                _ => "Unknown"
            }, 
            true,
            inquire.IsCompany);

        AddTokenToClient(_httpClient);

        var response = new HttpResponseMessage(HttpStatusCode.GatewayTimeout);
        var cancelToken = new CancellationTokenSource(30 * 1000);
        try
        {
            response = await _httpClient.PostAsJsonAsync("/Inquires", szymoInquiry, cancelToken.Token);
        }
        catch (TaskCanceledException e)
        {
            Console.WriteLine("CourierHubApi have not responded within 30 seconds: " + e.Message);
        }

        if (response.IsSuccessStatusCode)
        {
            var inquireResponse = await response.Content.ReadFromJsonAsync<SzymoInquireResponse>();
            if (inquireResponse != null)
            {
                var offer = new ApiOffer
                {
                    Price = (decimal)inquireResponse.totalPrice,
                    Code = inquireResponse.inquiryId,
                    ServiceName = ServiceName,
                    ExpirationDate = inquireResponse.expiringAt
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

    public async Task<int> PostOrder(ApiOrder order) {
        Console.WriteLine("PostOrder was invoked in SzymoHubApi.");

        var address = new SzymoAddress(
            order.ClientAddress.Number,
            order.ClientAddress.Flat,
            order.ClientAddress.Street,
            order.ClientAddress.PostalCode,
            order.ClientAddress.City,
            "Poland");

        var apiOrder = new SzymoPostOfferRequest(
            order.Code,
            order.ClientName,
            order.ClientEmail,
            address);

        AddTokenToClient(_httpClient);

        var response = new HttpResponseMessage(HttpStatusCode.GatewayTimeout);
        var cancelToken = new CancellationTokenSource(30 * 1000);
        try
        {
            response = await _httpClient.PostAsJsonAsync("/Offers", apiOrder, cancelToken.Token);
        }
        catch (TaskCanceledException e)
        {
            Console.WriteLine("CourierHubApi have not responded within 30 seconds: " + e.Message);
        }
        return (int)response.StatusCode;
    }

    public async Task<int> PutOrderWithrawal(string code) {
        Console.WriteLine("PutOrderWithrawal was invoked in SzymoHubApi.");

        AddTokenToClient(_httpClient);

        var response = new HttpResponseMessage(HttpStatusCode.GatewayTimeout);
        var cancelToken = new CancellationTokenSource(30 * 1000);
        try
        {
            response = await _httpClient.DeleteAsync($"/offer/{code}/cancel", cancelToken.Token);
        }
        catch (TaskCanceledException e)
        {
            Console.WriteLine("CourierHubApi have not responded within 30 seconds: " + e.Message);
        }
        return (int)response.StatusCode;
    }

    private bool AddTokenToClient(HttpClient client)
    {
        if(!_accessTokenContainer.IsServiceTokenCachedAndNotExpired(_service))
        {
            string[] userCredentials = _service.ApiKey.Split(';');
            if (userCredentials.Length < 2)
                return false;

            string clientId = userCredentials[0];
            string clientSecret = userCredentials[1];

            string? accessToken = _accessTokenContainer.GetToken(_service, clientId, clientSecret, _tokenEndPoint);
            if (accessToken == null)
                return false;

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }
        return true;
    }
}

class AccessTokenResponse
{
    public string? access_token { get; set; }

    // seems to be number of seconds untill the token is expired
    public int expires_in { get; set; } 
}
