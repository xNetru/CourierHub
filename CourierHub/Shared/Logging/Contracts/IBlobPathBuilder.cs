namespace CourierHub.Shared.Logging.Contracts {
    public enum Applications {
        CourierHub,
        API
    }
    public interface IBlobPathBuilder {
        void AddApplication(Applications application);
        void AddDate(DateOnly date);
        void AddController(string controllerName);
        void AddMethod(string methodName);
        void AddTime(TimeSpan time);
        void AddDefaultMethod();
        void AddDefaultController();
        void AddDateAndTime(DateTime datetime);
        void Reset();
        string? Build();
    }
}
