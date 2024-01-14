using CourierHub.Shared.Data;
using CourierHubWebApi.Services.Contracts;
using Microsoft.Extensions.Primitives;

namespace CourierHubWebApi.Services {
    public class ApiKeyService : IApiKeyService {
        private CourierHubDbContext _dbContext;
        private static Dictionary<string, int>? _apiKeyToServiceIdDictionary = null;
        private static readonly string _apiKeyName = "X-Api-Key";
        private static int _ourServiceId = 1;
        public string ApiKeyName { get => _apiKeyName; }
        public ApiKeyService(CourierHubDbContext dbContext, IConfiguration? configuration = null) {
            _dbContext = dbContext;
            if (_apiKeyToServiceIdDictionary == null) {
                FillDictionary();
                if(configuration != null )
                    SetOurServiceId(configuration);
            }

        }
        public bool TryGetServiceId(string apiKey, out int serviceId) {
            if (_apiKeyToServiceIdDictionary == null) {
                throw new InvalidOperationException("Trying to get value from not initialized object");
            }
            return _apiKeyToServiceIdDictionary.TryGetValue(apiKey, out serviceId);
        }
        public bool TryExtractApiKey(HttpContext context, out string apiKey) {
            if (context.Request.Headers.TryGetValue(_apiKeyName, out StringValues key)) {
                apiKey = key.ToString();
                return true;
            }
            apiKey = default!;
            return false;
        }
        public bool IsOurServiceRequest(int serviceId) {
            return serviceId == _ourServiceId;
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
                } catch { }
            }

        }
        private void SetOurServiceId(IConfiguration configuration) {
            if (_apiKeyToServiceIdDictionary != null) {
                string? serviceName = configuration["AppSettings:OurService"];
                if (serviceName != null)
                    _apiKeyToServiceIdDictionary.TryGetValue(serviceName, out _ourServiceId);

            }
        }
    }
}
