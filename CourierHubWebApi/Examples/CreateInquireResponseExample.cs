using CourierHubWebApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace CourierHubWebApi.Examples {
    public class CreateInquireResponseExample : IExamplesProvider<CreateInquireResponse> {
        public CreateInquireResponse GetExamples() {
            return new CreateInquireResponse(
                Price: 19.99m,
                Code: "MjAyNG0xbTExNjAwMDAw",
                ExpirationDate: DateTime.Now.AddMinutes(15));
        }
    }
}
