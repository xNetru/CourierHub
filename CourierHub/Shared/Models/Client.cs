using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.Models {
    public partial class Client : User {
<<<<<<< HEAD
        public virtual ClientData Data { get; set; } = null!;
=======
        [ValidateComplexType]
        public ClientData Data { get; set; } = null!;
>>>>>>> 28296336b69f8456147901bf8b138a44ba40ba84
    }
}
