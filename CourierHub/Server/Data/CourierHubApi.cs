using CourierHub.Shared.Abstractions;
using CourierHub.Shared.ApiModels;
using CourierHub.Shared.Enums;

namespace CourierHub.Server.Data {
    public class CourierHubApi : IWebApi {
        public async Task<(StatusType?, string)> GetOrderStatus(string code) {
            Console.WriteLine("GetOrderStatus was invoked in CourierHubApi.");
            return (null, "400");
        }

        public async Task<(ApiOffer?, string)> PostInquireGetOffer(ApiInquire inquire) {
            Console.WriteLine("PostInquireGetOffer was invoked in CourierHubApi.");
            return (null, "400");
        }

        public async Task<string> PostOrder(ApiOrder order) {
            Console.WriteLine("PostOrder was invoked in CourierHubApi.");
            return "400";
        }

        public async Task<string> PutOrderWithrawal(string code) {
            Console.WriteLine("PutOrderWithrawal was invoked in CourierHubApi.");
            return "400";
        }
    }
}
