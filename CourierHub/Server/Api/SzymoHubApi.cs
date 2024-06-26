﻿using CourierHub.Api.Models.SzymoHubApi;
using CourierHub.Server.Api.Models.SzymoHubApi;
using CourierHub.Server.Validation;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Enums;
using System.Net;

namespace CourierHub.Server.Api;
public class SzymoHubApi : IWebApi {
    private readonly ApiService _service;
    private readonly HttpClient _httpClient = new();
    private readonly AccessTokenContainer _accessTokenContainer;
    private static readonly string _tokenBase = "https://indentitymanager.snet.com.pl/";
    private static readonly string _tokenEndPoint = "/connect/token";
    private SzymoInquiryValidator inquiryValidator = new();
    private SzymoPostOfferRequestValidator offerValidator = new();
    public string ServiceName { get; set; }

    public SzymoHubApi(ApiService service, AccessTokenContainer accessTokenContainer) {
        _service = service;
        ServiceName = _service.Name;
        _httpClient.BaseAddress = new Uri(_service.BaseAddress);
        _accessTokenContainer = accessTokenContainer;
    }

    public async Task<(StatusType?, int, string?)> GetOrderStatus(string code) {
        Console.WriteLine("GetOrderStatus was invoked in SzymoHubApi.");

        AddTokenToClient(_httpClient);

        SzymoGetOfferStatusResponse? response = null;
        var cancelToken = new CancellationTokenSource(30 * 1000);
        try {
            response = await _httpClient.GetFromJsonAsync<SzymoGetOfferStatusResponse?>($"/offer/request/{code}/status", cancelToken.Token);
        } catch (TaskCanceledException e) {
            Console.WriteLine("SzymoHubApi have not responded within 30 seconds: " + e.Message);
        } catch (Exception e) {
            Console.WriteLine("[SzymoHubApi]: Error has occured: " + e.Message);
        }

        if (response == null) {
            var getResponse = await _httpClient.GetFromJsonAsync<SzymoGetOfferResponse>($"/offer/{code}", cancelToken.Token);

            if (getResponse != null)
                return getResponse.offerStatus switch
                {
                    Enums.SzymoHubApi.OfferStatus.Accepted => (StatusType.Confirmed, StatusCodes.Status200OK, null),
                    _ => (StatusType.Cancelled, StatusCodes.Status200OK, null)
                };
                 
        } else {
            if (response.isReady) {
                return (StatusType.Confirmed, StatusCodes.Status200OK, response.offerId);
            } else {
                return (StatusType.NotConfirmed, StatusCodes.Status200OK, null);
            }
        }
        return (null, StatusCodes.Status503ServiceUnavailable, null);
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
            inquire.Priority switch {
                (int)PriorityType.High => "High",
                (int)PriorityType.Low => "Low",
                (int)PriorityType.Medium => "Medium",
                _ => "Unknown"
            },
            true,
            inquire.IsCompany);

        if (!inquiryValidator.Validate(szymoInquiry).IsValid)
            return (null, StatusCodes.Status400BadRequest);

        AddTokenToClient(_httpClient);

        var response = new HttpResponseMessage(HttpStatusCode.GatewayTimeout);
        var cancelToken = new CancellationTokenSource(30 * 1000);
        try {
            response = await _httpClient.PostAsJsonAsync("/Inquires", szymoInquiry, cancelToken.Token);
        } catch (TaskCanceledException e) {
            Console.WriteLine("SzymoHubApi have not responded within 30 seconds: " + e.Message);
        } catch (Exception e) {
            Console.WriteLine("[SzymoHubApi]: Error has occured: " + e.Message);
        }

        if (response.IsSuccessStatusCode) {
            var inquireResponse = await response.Content.ReadFromJsonAsync<SzymoInquireResponse>();
            if (inquireResponse != null) {
                var offer = new ApiOffer {
                    Price = (decimal)inquireResponse.totalPrice,
                    Code = inquireResponse.inquiryId,
                    ServiceName = ServiceName,
                    ExpirationDate = inquireResponse.expiringAt
                };
                return (offer, (int)response.StatusCode);
            } else {
                return (null, StatusCodes.Status503ServiceUnavailable);
            }
        } else {
            return (null, (int)response.StatusCode);
        }
    }

    public async Task<(int, string?)> PostOrder(ApiOrder order) {
        Console.WriteLine("PostOrder was invoked in SzymoHubApi.");

        var address = new SzymoAddress(
            order.ClientAddress.Number,
            order.ClientAddress.Flat,
            order.ClientAddress.Street,
            order.ClientAddress.City,
            order.ClientAddress.PostalCode,
            "Poland");

        var szymoOffer = new SzymoPostOfferRequest(
            order.Code,
            order.ClientName,
            order.ClientEmail,
            address);

        if (!offerValidator.Validate(szymoOffer).IsValid)
            return (StatusCodes.Status400BadRequest, null);

        AddTokenToClient(_httpClient);

        var response = new HttpResponseMessage(HttpStatusCode.GatewayTimeout);
        var cancelToken = new CancellationTokenSource(30 * 1000);
        try {
            response = await _httpClient.PostAsJsonAsync("/Offers", szymoOffer, cancelToken.Token);
        } catch (TaskCanceledException e) {
            Console.WriteLine("SzymoHubApi have not responded within 30 seconds: " + e.Message);
        } catch (Exception e) {
            Console.WriteLine("[SzymoHubApi]: Error has occured: " + e.Message);
        }

        if (response.IsSuccessStatusCode) {
            var orderResponse = await response.Content.ReadFromJsonAsync<SzymoPostOfferResponse>();
            if (orderResponse != null) {
                return ((int)response.StatusCode, orderResponse.offerRequestId);
            } else {
                return (StatusCodes.Status503ServiceUnavailable, null);
            }
        }
        return ((int)response.StatusCode, null);
    }

    public async Task<int> PutOrderWithrawal(string code) {
        Console.WriteLine("PutOrderWithrawal was invoked in SzymoHubApi.");

        AddTokenToClient(_httpClient);

        var response = new HttpResponseMessage(HttpStatusCode.GatewayTimeout);
        var cancelToken = new CancellationTokenSource(30 * 1000);
        try {
            response = await _httpClient.DeleteAsync($"/offer/{code}/cancel", cancelToken.Token);
        } catch (TaskCanceledException e) {
            Console.WriteLine("SzymoHubApi have not responded within 30 seconds: " + e.Message);
        } catch (Exception e) {
            Console.WriteLine("[SzymoHubApi]: Error has occured: " + e.Message);
        }
        return (int)response.StatusCode;
    }

    private bool AddTokenToClient(HttpClient client) {
        string[] userCredentials = _service.ApiKey.Split(';');
        if (userCredentials.Length < 2)
            return false;

        string clientId = userCredentials[0];
        string clientSecret = userCredentials[1];

        string? accessToken = _accessTokenContainer.GetToken(_service, clientId, clientSecret, _tokenBase, _tokenEndPoint);
        if (accessToken == null)
            return false;

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        return true;
    }
}
