using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierHub.Shared.Models;

public partial class Review {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int Value { get; set; }

    public string? Description { get; set; }

    public DateTime Datetime { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
