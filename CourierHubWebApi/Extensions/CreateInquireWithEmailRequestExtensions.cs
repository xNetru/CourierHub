using CourierHub.Shared.Models;
using CourierHubWebApi.Models;

namespace CourierHubWebApi.Extensions
{
    public static class CreateInquireWithEmailRequestExtensions
    {
        public static Inquire CreateInquire(this CreateInquireWithEmailRequest request)
        {
            return request.standardRequest.CreateInquire();
        }
    }
}
