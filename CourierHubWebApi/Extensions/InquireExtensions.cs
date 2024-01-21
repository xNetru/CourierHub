using CourierHub.Shared.Models;
using CourierHubWebApi.Models;

namespace CourierHubWebApi.Extensions {
    public static class InquireExtensions {
        public static Inquire CreateInquire(this CreateInquireRequest request) {
            Inquire inquire = new();
            // inquire.ClientId = request.ClientId;
            inquire.Depth = request.Depth;
            inquire.Width = request.Width;
            inquire.Length = request.Length;
            inquire.Mass = request.Mass;
            inquire.SourceDate = request.SourceDate;
            inquire.DestinationDate = request.DestinationDate;
            inquire.Datetime = request.Datetime;
            inquire.IsCompany = request.IsCompany;
            inquire.IsWeekend = request.IsWeekend;
            inquire.Priority = (int)request.Priority;
            inquire.Source = request.CreateSourceAddress();
            inquire.Destination = request.CreateDestinationAddress();

            return inquire;
        }
        public static Address CreateSourceAddress(this CreateInquireRequest request) {
            return request.SourceAddress.CreateEntityAddress();
        }
        public static Address CreateDestinationAddress(this CreateInquireRequest request) {
            return request.DestinationAddress.CreateEntityAddress();
        }
    }
}
