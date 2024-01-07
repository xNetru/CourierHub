using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierHub.Shared.Models;

public partial class Service {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ApiKey { get; set; } = null!;

    public string Statute { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
