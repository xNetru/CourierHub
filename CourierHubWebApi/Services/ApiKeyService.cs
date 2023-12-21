using CourierHub.Shared.Data;
using CourierHubWebApi.Services.Contracts;

namespace CourierHubWebApi.Services {
    public class ApiKeyService : IApiKeyService {
        private CourierHubDbContext _dbContext;
        private static Dictionary<string, int>? _apiKeyToServiceIdDictionary = null;
        public ApiKeyService(CourierHubDbContext dbContext) {
            _dbContext = dbContext;
            if (_apiKeyToServiceIdDictionary == null)
                FillDictionary();
        }
        public bool TryGetServiceId(string ApiKey, out int ServiceId) {
            if (_apiKeyToServiceIdDictionary == null) {
                throw new InvalidOperationException("Trying to get value from not initialized object");
            }
            return _apiKeyToServiceIdDictionary.TryGetValue(ApiKey, out ServiceId);
        }
        private void FillDictionary() {
            if (_apiKeyToServiceIdDictionary == null) {
                _apiKeyToServiceIdDictionary = new Dictionary<string, int>();
                try {
                    var services = from service
                                   in _dbContext.Services
                                   select service;
                    foreach (var service in services) {
                        _apiKeyToServiceIdDictionary.Add(service.ApiKey, service.Id);
                    }
                } catch {
                    // TODO: Database error handling
                }

            }

        }
    }
}
