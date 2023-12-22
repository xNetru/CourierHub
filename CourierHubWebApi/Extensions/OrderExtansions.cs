using CourierHub.Shared.Models;
using CourierHubWebApi.Models;
using System.Runtime.CompilerServices;

namespace CourierHubWebApi.Extensions
{
    public static class OrderExtansions
    {
        public static Order CreateOrder(this CreateOrderRequest request)
        {
            Order order = new Order();
            order.ClientName = request.ClientName;
            order.ClientSurname = request.ClientSurname;
            order.ClientEmail = request.ClientEmail;
            order.ClientPhone = request.ClientPhoneNumber;
            order.ClientCompany = request.ClientCompany;
            return order;
        }
        public static Address CreateClientAddress(this CreateOrderRequest request)
        {
            Address clientAddress = new Address();
            clientAddress.City = request.ClientCity;
            clientAddress.Street = request.ClientStreet;
            clientAddress.Number = request.ClientAddressBuildingNumber;
            clientAddress.Flat = request.ClientFlat;
            clientAddress.PostalCode = request.ClientPostalCode;
            return clientAddress;
        }
    }
}
