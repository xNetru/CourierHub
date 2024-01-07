using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierHub.Shared.Models;

public partial class Status {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsCancelable { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
