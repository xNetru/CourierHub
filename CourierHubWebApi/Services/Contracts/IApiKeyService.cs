namespace CourierHubWebApi.Services.Contracts {
    public interface IApiKeyService {
        bool TryGetServiceId(string ApiKey, out int ServiceId);
        bool TryExtractApiKey(HttpContext context, out string ApiKey);
        bool IsOurServiceRequest(int serviceId);
        string ApiKeyName { get; }
    }
}
