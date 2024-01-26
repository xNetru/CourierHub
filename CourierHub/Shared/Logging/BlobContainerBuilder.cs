using CourierHub.Shared.Logging.Contracts;

namespace CourierHub.Shared.Logging {
    public class BlobContainerBuilder : IBlobContainerBuilder {
        private string _blobContainerName = string.Empty;
        public void AddLogs() {
            _blobContainerName = new string("logs");
        }
        public string? Build() {
            return _blobContainerName.Length > 0 ? new string(_blobContainerName) : default;
        }
        public void Reset() {
            _blobContainerName = string.Empty;
        }
    }
}
