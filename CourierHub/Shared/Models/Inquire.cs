using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierHub.Shared.Models;

public partial class Inquire
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? ClientId { get; set; }

    [Required(ErrorMessage = "Głębokość paczki jest wymagana")]
    [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Głębokość musi być liczbą bez wiodących 0")]
    public int Depth { get; set; }

    [Required(ErrorMessage = "Szerokość paczki jest wymagana")]
    [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Szerokość musi być liczbą bez wiodących 0")]
    public int Width { get; set; }

    [Required(ErrorMessage = "Długość paczki jest wymagana")]
    [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Długość musi być liczbą bez wiodących 0")]
    public int Length { get; set; }

    [Required(ErrorMessage = "Masa paczki jest wymagana")]
    [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Masa musi być liczbą bez wiodących 0")]
    public int Mass { get; set; }

    public int SourceId { get; set; }

    public int DestinationId { get; set; }

    [Required(ErrorMessage = "Data wysyłki jest wymagana")]
    public DateTime SourceDate { get; set; }

    [Required(ErrorMessage = "Data dostarczenia jest wymagana")]
    public DateTime DestinationDate { get; set; }

    public DateTime Datetime { get; set; }

    public bool IsCompany { get; set; }

    public bool IsWeekend { get; set; }

    public int Priority { get; set; }

    [ValidateComplexType]
    public virtual Address Destination { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    [ValidateComplexType]
    public virtual Address Source { get; set; } = null!;

    public virtual User? Client { get; set; }
}
