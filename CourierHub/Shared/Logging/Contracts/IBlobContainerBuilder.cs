namespace CourierHub.Shared.Logging.Contracts {
    public interface IBlobContainerBuilder {
        void AddLogs();
        string? Build();
        void Reset();
    }
}
