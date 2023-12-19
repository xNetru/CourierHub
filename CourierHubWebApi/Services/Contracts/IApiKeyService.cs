namespace CourierHubWebApi.Services.Contracts
{
    public interface IApiKeyService
    {
        bool TryGetServiceId(string ApiKey, out int ServiceId);
    }
}
