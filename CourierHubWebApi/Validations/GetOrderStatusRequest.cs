using Elasticsearch.Net.Specification.SnapshotLifecycleManagementApi;
using FluentValidation;
using CourierHubWebApi.Models;

namespace CourierHubWebApi.Validations
{
    public class GetOrderStatusRequestValidator:AbstractValidator<GetOrderStatusRequest>
    {
        private OrderCodeValidator _orderCodeValidator = new OrderCodeValidator();
        public GetOrderStatusRequestValidator()
        {
            RuleFor(x => x.Code).SetValidator(_orderCodeValidator);
        }
    }
}
