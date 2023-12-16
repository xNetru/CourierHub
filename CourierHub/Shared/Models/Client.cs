namespace CourierHub.Shared.Models {
    public partial class Client : User {
        public virtual ClientData Data { get; set; } = null!;
    }
}
