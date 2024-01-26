namespace CourierHub.Shared.Logging.Contracts {
    public interface ILogger {
        IBlobData blobData { get; }
        Task<bool> SaveLog();
    }
}
