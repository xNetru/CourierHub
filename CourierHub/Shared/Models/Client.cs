namespace CourierHub.Shared.Models {
    public partial class Client : User {
        public ClientData Data { get; set; } = null!;
    }
}
