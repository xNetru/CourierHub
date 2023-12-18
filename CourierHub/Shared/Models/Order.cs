using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierHub.Shared.Models;

public partial class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int InquireId { get; set; }

    public decimal Price { get; set; }

    public int StatusId { get; set; }

    public int ServiceId { get; set; }

    public int? EvaluationId { get; set; }

    public int? ParcelId { get; set; }

    public int? ReviewId { get; set; }

    [Required(ErrorMessage = "Email jest wymagany")]
    [EmailAddress(ErrorMessage = "Niepoprawny format maila")]
    public string ClientEmail { get; set; } = null!;

    [Required(ErrorMessage = "Imię jest wymagane")]
    [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Imię musi się zaczynać od duzej litery")]
    public string ClientName { get; set; } = null!;

    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Nazwisko musi się zaczynać od duzej litery")]
    public string ClientSurname { get; set; } = null!;

    [Phone]
    public string ClientPhone { get; set; } = null!;

    public string ClientCompany { get; set; } = null!;

    public int ClientAddressId { get; set; }

    public virtual Address? ClientAddress { get; set; } = null!;

    public virtual Evaluation? Evaluation { get; set; }

    public virtual Inquire? Inquire { get; set; } = null!;

    public virtual Parcel? Parcel { get; set; }

    public virtual Review? Review { get; set; }

    public virtual Service? Service { get; set; } = null!;

    public virtual Status? Status { get; set; } = null!;
}
