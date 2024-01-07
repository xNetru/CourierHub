﻿using CourierHub.CourierHubApiModels;
using CourierHub.Shared.Abstractions;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Enums;
using System.Net;

namespace CourierHub.Server.Data; 
public class CourierHubApi : IWebApi {
    private readonly HttpClient _httpClient = new();

    public string ServiceName { get; set; }

    public CourierHubApi(ApiService service) {
        ServiceName = service.Name;
        _httpClient.BaseAddress = new Uri(service.BaseAddress);
        _httpClient.DefaultRequestHeaders.Add("x-api-key", service.ApiKey);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<(StatusType?, int)> GetOrderStatus(string code) {
        Console.WriteLine("GetOrderStatus was invoked in CourierHubApi.");
        return (null, 400);
        // Kamilu pamiętaj o 30s limitu (wzoruj się na gotowych implementacjach)
    }

    public async Task<(ApiOffer?, int)> PostInquireGetOffer(ApiInquire inquire) {
        Console.WriteLine("PostInquireGetOffer was invoked in CourierHubApi.");

        var source = new ApiSideAddress(
            inquire.Source.City,
            inquire.Source.PostalCode,
            inquire.Source.Street,
            inquire.Source.Number,
            inquire.Source.Flat);

        var destination = new ApiSideAddress(
            inquire.Destination.City,
            inquire.Destination.PostalCode,
            inquire.Destination.Street,
            inquire.Destination.Number,
            inquire.Destination.Flat);

        var apiInquire = new CreateInquireRequest(
            inquire.Depth,
            inquire.Width,
            inquire.Length,
            inquire.Mass,
            source,
            destination,
            inquire.SourceDate,
            inquire.DestinationDate,
            inquire.Datetime,
            inquire.IsCompany,
            inquire.IsWeekend,
            inquire.Priority);

        var response = new HttpResponseMessage(HttpStatusCode.GatewayTimeout);
        var cancelToken = new CancellationTokenSource(30 * 1000);
        try {
            if (inquire.Email != null) {
                var apiInquireWithMail = new CreateInquireWithEmailRequest(apiInquire, inquire.Email);
                response = await _httpClient.PostAsJsonAsync("/api/Inquire", apiInquireWithMail, cancelToken.Token);
            } else {
                response = await _httpClient.PostAsJsonAsync("/api/Inquire", apiInquire, cancelToken.Token);
            }
        } catch (TaskCanceledException e) {
            Console.WriteLine("CourierHubApi have not responded within 30 seconds: " + e.Message);
        }

        if (response.IsSuccessStatusCode) {
            var inquireResponse = await response.Content.ReadFromJsonAsync<CreateInquireResponse>();
            if (inquireResponse != null) {
                var offer = new ApiOffer {
                    Price = inquireResponse.Price,
                    Code = inquireResponse.Code,
                    ExpirationDate = inquireResponse.ExpirationDate
                };
                return (offer, (int)response.StatusCode);
            } else {
                return (null, 503);
            }
        } else {
            return (null, (int)response.StatusCode);
        }
    }

    public async Task<int> PostOrder(ApiOrder order) {
        Console.WriteLine("PostOrder was invoked in CourierHubApi.");

        var address = new ApiSideAddress(
            order.ClientAddress.City,
            order.ClientAddress.PostalCode,
            order.ClientAddress.Street,
            order.ClientAddress.Number,
            order.ClientAddress.Flat);

        var apiOrder = new CreateOrderRequest(
            order.Code,
            order.ClientName,
            order.ClientSurname,
            order.ClientEmail,
            order.ClientPhone,
            order.ClientCompany,
            address);

        var response = new HttpResponseMessage(HttpStatusCode.GatewayTimeout);
        var cancelToken = new CancellationTokenSource(30 * 1000);
        try {
            response = await _httpClient.PostAsJsonAsync("/api/Order", apiOrder, cancelToken.Token);
        } catch (TaskCanceledException e) {
            Console.WriteLine("CourierHubApi have not responded within 30 seconds: " + e.Message);
        }
        return (int)response.StatusCode;
    }

    public async Task<int> PutOrderWithrawal(string code) {
        Console.WriteLine("PutOrderWithrawal was invoked in CourierHubApi.");
        return 400;
        // Kamilu pamiętaj o 30s limitu (wzoruj się na gotowych implementacjach)
    }
}
