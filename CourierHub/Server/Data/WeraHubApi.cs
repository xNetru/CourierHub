﻿using CourierHub.Shared.Abstractions;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Enums;

namespace CourierHub.Server.Data; 
public class WeraHubApi : IWebApi {
    private readonly ApiService _service;

    public string ServiceName { get; set; }

    public WeraHubApi(ApiService service) {
        _service = service;
        ServiceName = _service.Name;
    }
    public async Task<(StatusType?, int)> GetOrderStatus(string code) {
        Console.WriteLine("GetOrderStatus was invoked in WeronikaHubApi.");
        return (null, 400);
    }

    public async Task<(ApiOffer?, int)> PostInquireGetOffer(ApiInquire inquire) {
        Console.WriteLine("PostInquireGetOffer was invoked in WeronikaHubApi.");
        return (null, 400);
    }

    public async Task<int> PostOrder(ApiOrder order) {
        Console.WriteLine("PostOrder was invoked in WeronikaHubApi.");
        return 400;
    }

    public async Task<int> PutOrderWithrawal(string code) {
        Console.WriteLine("PutOrderWithrawal was invoked in WeronikaHubApi.");
        return 400;
    }
}
