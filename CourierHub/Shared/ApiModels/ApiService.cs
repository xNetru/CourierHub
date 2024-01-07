using CourierHub.Shared.Models;

namespace CourierHub.Shared.ApiModels;

public class ApiService {
    public string Name { get; set; } = null!;

    public string ApiKey { get; set; } = null!;

    public string Statute { get; set; } = null!;

    public string BaseAddress { get; set; } = null!;

    public static explicit operator ApiService(Service service) {
        return new ApiService {
            Name = service.Name,
            ApiKey = service.ApiKey,
            Statute = service.Statute
        };
    }
}
