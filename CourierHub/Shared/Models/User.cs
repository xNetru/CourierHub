namespace CourierHub.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public abstract partial class User {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Imię jest wymagane")]
    [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Imię musi się zaczynać od duzej litery")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Nazwisko musi się zaczynać od duzej litery")]
    public string Surname { get; set; } = null!;

    public int Type { get; set; }

    public virtual ICollection<ClientData> ClientData { get; } = new List<ClientData>();

    public virtual ICollection<Evaluation> Evaluations { get; } = new List<Evaluation>();

    public virtual ICollection<Inquire> Inquires { get; } = new List<Inquire>();

    public virtual ICollection<Parcel> Parcels { get; } = new List<Parcel>();
}
