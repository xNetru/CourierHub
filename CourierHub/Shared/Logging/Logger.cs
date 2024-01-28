using CourierHub.Cloud;
using CourierHub.Shared.Logging.Contracts;

namespace CourierHub.Shared.Logging {
    public class Logger : ILogger {
        private IBlobData _blobData = new BlobData();
        private ICloudStorage _azureStorage;
        public IBlobData blobData { get => _blobData; }
        public Logger(ICloudStorage azureStorage) {
            _azureStorage = azureStorage;
        }
        public async Task<bool> SaveLog() {
            string? path = _blobData.Path;
            string? container = _blobData.Container;
            string blob = _blobData.Blob;

            if (container == null || path == null) {
                return false;
            }
            try {
                await _azureStorage.PutBlobAsync((string)path, (string)container, blob, true);
                return true;
            } catch {
                return false;
            }
        }
    }
}
