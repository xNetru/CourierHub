using CourierHubWebApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace CourierHubWebApi.Examples {
    public class CreateOrderRequestExample : IExamplesProvider<CreateOrderRequest> {
        CreateOrderRequest IExamplesProvider<CreateOrderRequest>.GetExamples() {
            ApiSideAddress clientAddress = new ApiSideAddress(
                City: "Bielsko-Biała",
                PostalCode: "29-102",
                Street: "Krakowska",
                Number: "2a",
                Flat: "32b");

            CreateOrderRequest request = new CreateOrderRequest(
                InquireCode: "MjAyNG0xbTExNjAwMDAw",
                ClientName: "Wiesław",
                ClientSurname: "Nowak",
                ClientEmail: "example@ex.pl",
                ClientPhoneNumber: "987827918",
                ClientCompany: "CourierHub",
                ClientAddress: clientAddress);

            return request;
        }
    }
}
