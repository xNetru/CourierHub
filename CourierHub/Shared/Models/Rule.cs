using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.Models;

public partial class Rule {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? DepthMax { get; set; }

    public int? WidthMax { get; set; }

    public int? LengthMax { get; set; }

    public int? MassMax { get; set; }

    public float? VelocityMax { get; set; }
}
