using CourierHubWebApi.Models;
using Swashbuckle.AspNetCore.Filters;
using CourierHub.Shared.Enums;

namespace CourierHubWebApi.Examples
{
    public class CreateInquireRequestExample : IExamplesProvider<CreateInquireRequest>
    {
        CreateInquireRequest IExamplesProvider<CreateInquireRequest>.GetExamples()
        {
            ApiSideAddress source = new ApiSideAddress(
                City: "Lipinki Łużyckie",
                PostalCode: "01-912",
                Street: "Rozłączna",
                Number: "13",
                Flat: "23a");

            ApiSideAddress destination = new ApiSideAddress(
                City: "Węgorzewo",
                PostalCode: "02-102",
                Street: "Śląska",
                Number: "5",
                Flat: null);

            DateTime sourceDate = DateTime.Now.AddDays(20);
            DateTime destinationDate = sourceDate.AddDays(3);

            CreateInquireRequest inquiry = new CreateInquireRequest(
                Depth: 100,
                Width: 100,
                Length: 100,
                Mass: 300,
                SourceAddress: source,
                DestinationAddress: destination,
                SourceDate: sourceDate,
                DestinationDate: destinationDate,
                Datetime: DateTime.Now,
                IsCompany: true,
                IsWeekend: destinationDate.DayOfWeek == DayOfWeek.Saturday ||
                destinationDate.DayOfWeek == DayOfWeek.Sunday,
                Priority: PriorityType.High);

            return inquiry;
        }
    }
}
