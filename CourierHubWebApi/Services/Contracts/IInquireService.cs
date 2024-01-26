using CourierHubWebApi.Errors;
using CourierHubWebApi.Models;
using OneOf;

namespace CourierHubWebApi.Services.Contracts {
    public interface IInquireService {
        Task<OneOf<CreateInquireResponse, ApiError>> CreateInquire(CreateInquireRequest inquire, int serviceId);
    }
}
