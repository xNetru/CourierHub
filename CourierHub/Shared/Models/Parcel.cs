using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.Models;

public partial class Parcel {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTime? PickupDatetime { get; set; }

    public DateTime? DeliveryDatetime { get; set; }

    public string? UndeliveredReason { get; set; }

    public int CourierId { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual User Courier { get; set; } = null!;
}
