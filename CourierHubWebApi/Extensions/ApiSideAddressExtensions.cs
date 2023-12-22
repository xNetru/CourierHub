using CourierHub.Shared.Models;
using CourierHubWebApi.Models;
using System.Text;

namespace CourierHubWebApi.Extensions
{
    public static class ApiSideAddressExtensions
    {
        public static Address CreateEntityAddress(this ApiSideAddress apiSideAddress)
        {
            Address address= new Address();
            address.City = apiSideAddress.City;
            StringBuilder postalCodeBuilder = new StringBuilder();
            string[] splittedPostalCode = apiSideAddress.PostalCode.Split('-');
            postalCodeBuilder.Append(splittedPostalCode[0]);
            postalCodeBuilder.Append(splittedPostalCode[1]);
            address.PostalCode = postalCodeBuilder.ToString();
            address.Street = apiSideAddress.Street;
            address.Number = apiSideAddress.Number; 
            address.Flat = apiSideAddress.Flat; 
            return address;
        }
    }
}
