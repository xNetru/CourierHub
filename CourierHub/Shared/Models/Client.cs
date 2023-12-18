using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.Models {
    public partial class Client : User {

        [ValidateComplexType]
        public ClientData Data { get; set; } = null!;
    }
}