using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierHub.Shared.Models
{
    public partial class Client : User
    {
        [ValidateComplexType]
        public ClientData Data { get; set; } = null!;
    }
}