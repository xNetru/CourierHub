using CourierHubWebApi.Models;
using OneOf;
using CourierHubWebApi.Errors;

namespace CourierHubWebApi.Services.Contracts {
    public interface IInquireService {
        // Task<ErrorOr<CreateInquireResponse>> CreateInquire(CreateInquireRequest inquire, int serviceId);
        // Task<ErrorOr<CreateInquireResponse>> CreateInquireWithEmail(CreateInquireWithEmailRequest reuqest, int serviceId);

        Task<OneOf<CreateInquireResponse,ApiError>> CreateInquire(CreateInquireRequest inquire, int serviceId);
    }
}
