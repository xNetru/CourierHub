using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Enums;

namespace CourierHub.Server.Api;
public interface IWebApi {
    public string ServiceName { get; set; }

    // client sends inquire and gets an offer and status
    Task<(ApiOffer?, int)> PostInquireGetOffer(ApiInquire inquire);

    // client creates and sends an order, receives status
    Task<(int, string?)> PostOrder(ApiOrder order);

    // client withraws the order, receives status
    Task<int> PutOrderWithrawal(string code);

    // client checks the status of order, receives status
    Task<(StatusType?, int, string?)> GetOrderStatus(string code);
}