using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Enums;

namespace CourierHub.Shared.Abstractions {
    public interface IWebApi {
        // client sends inquire and gets an offer and message
        Task<(ApiOffer?, string)> PostInquireGetOffer(ApiInquire inquire);

        // client creates and sends an order, receives message
        Task<string> PostOrder(ApiOrder order);

        // client withraws the order, receives message
        Task<string> PutOrderWithrawal(string code);

        // client checks the status of order, receives message
        Task<(StatusType?, string)> GetOrderStatus(string code);
    }
}