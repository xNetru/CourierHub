namespace CourierHub.Shared.Logging.Contracts {
    public interface IBlobBuilder {
        void AddStatusCode(int statusCode);
        void AddRequest(object request);
        void AddResponse(object response);
        void AddError(object error);
        void AddOperationTime(ulong time);
        string Build();
        void Reset();
    }
}
