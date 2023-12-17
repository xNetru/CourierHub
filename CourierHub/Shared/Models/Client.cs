using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.Models {
    public partial class Client : User {
<<<<<<< HEAD
<<<<<<< HEAD
        public virtual ClientData Data { get; set; } = null!;
=======
        [ValidateComplexType]
        public ClientData Data { get; set; } = null!;
>>>>>>> 28296336b69f8456147901bf8b138a44ba40ba84
=======
        [ValidateComplexType]
        public ClientData Data { get; set; } = null!;
>>>>>>> 617ad29b7bf85550604dda58be4ccf9a5f4f6de1
    }
}