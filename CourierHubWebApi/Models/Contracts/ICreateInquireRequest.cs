using CourierHub.Shared.Models;

namespace CourierHubWebApi.Models.Contracts {
    public interface ICreateInquireRequest {
        public Inquire CreateInquire();
    }
}
