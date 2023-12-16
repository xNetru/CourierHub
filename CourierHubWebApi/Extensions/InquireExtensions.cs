using CourierHub.Shared.Models;
using CourierHubWebApi.Models;

namespace CourierHubWebApi.Extensions {
    public static class InquireExtensions {
        public static Inquire CreateInquire(this CreateInquireRequest request) {
            Inquire inquire = new();
            // inquire.Id = 0;
            inquire.ClientId = request.ClientId;
            inquire.Depth = request.Depth;
            inquire.Width = request.Width;
            inquire.Length = request.Length;
            inquire.Mass = request.Mass;
            inquire.SourceDate = request.SourceDate;
            inquire.DestinationDate = request.DestinationDate;
            inquire.Datetime = request.Datetime;
            inquire.IsCompany = request.IsCompany;
            inquire.IsWeekend = request.IsWeekend;
            inquire.Priority = request.Priority;
            return inquire;
        }
        public static Address CreateSourceAddress(this CreateInquireRequest request) {
            Address sourceAddress = new();
            // sourceAddress.Id = 0;   
            sourceAddress.Street = request.SourceStreet;
            sourceAddress.Number = request.SourceNumber;
            sourceAddress.Flat = request.SourceFlat;
            sourceAddress.PostalCode = request.SourcePostalCode;
            return sourceAddress;
        }
        public static Address CreateDestinationAddress(this CreateInquireRequest request) {
            Address destinationAddress = new();
            // destinationAddress.Id = 0;
            destinationAddress.Street = request.DestinationStreet;
            destinationAddress.Number = request.DestinationNumber;
            destinationAddress.Flat = request.DestinationFlat;
            destinationAddress.PostalCode = request.DestinationPostalCode;
            return destinationAddress;
        }
    }
}
