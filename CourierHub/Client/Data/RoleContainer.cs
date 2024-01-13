namespace CourierHub.Client.Data {
    public class RoleContainer {
        public Dictionary<string, bool> Roles { get; set; } = new Dictionary<string, bool>
        {
            { "NotAuthorized", true},
            { "Client", false},
            { "OfficeWorker", false},
            { "Courier", false}
        };
    }
}
