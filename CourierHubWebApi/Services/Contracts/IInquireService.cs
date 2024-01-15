using CourierHubWebApi.Models;
using ErrorOr;
using OneOf;

namespace CourierHubWebApi.Services.Contracts {
    public interface IInquireService {
        // Task<ErrorOr<CreateInquireResponse>> CreateInquire(CreateInquireRequest inquire, int serviceId);
        // Task<ErrorOr<CreateInquireResponse>> CreateInquireWithEmail(CreateInquireWithEmailRequest reuqest, int serviceId);

        Task<OneOf<CreateInquireResponse,int>> CreateInquire(CreateInquireRequest inquire, int serviceId);
    }
}
