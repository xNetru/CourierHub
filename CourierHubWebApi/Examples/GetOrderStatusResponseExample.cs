using CourierHub.Shared.Enums;
using Swashbuckle.AspNetCore.Filters;

namespace CourierHubWebApi.Examples {
    public class GetOrderStatusResponseExample : IExamplesProvider<StatusType> {
        StatusType IExamplesProvider<StatusType>.GetExamples() {
            return StatusType.NotConfirmed;
        }
    }
}
