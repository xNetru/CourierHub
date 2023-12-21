using CourierHub.Shared.Models;
using CourierHubWebApi.Models;

namespace CourierHubWebApi.Extensions
{
    public static class ApiSideAddressExtensions
    {
        public static Address CreateEntityAddress(this ApiSideAddress apiSideAddress)
        {
            Address address= new Address();
            address.City = apiSideAddress.City;
            address.PostalCode = apiSideAddress.PostalCode;
            address.Street = apiSideAddress.Street;
            address.Number = apiSideAddress.Number; 
            address.Flat = apiSideAddress.Flat; 
            return address;
        }
    }
}
