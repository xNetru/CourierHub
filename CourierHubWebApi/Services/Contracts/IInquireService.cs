using CourierHubWebApi.Models;
using OneOf;
using CourierHubWebApi.Errors;

namespace CourierHubWebApi.Services.Contracts {
    public interface IInquireService {
        Task<OneOf<CreateInquireResponse,ApiError>> CreateInquire(CreateInquireRequest inquire, int serviceId);
    }
}
