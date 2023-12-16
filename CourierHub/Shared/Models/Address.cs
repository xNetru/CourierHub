using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierHub.Shared.Models;

public partial class Address {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; } = null;

    public string Street { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string Flat { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public virtual ICollection<ClientData> ClientDatumAddresses { get; } = new List<ClientData>();

    public virtual ICollection<ClientData> ClientDatumSourceAddresses { get; } = new List<ClientData>();

    public virtual ICollection<Inquire> InquireDestinations { get; } = new List<Inquire>();

    public virtual ICollection<Inquire> InquireSources { get; } = new List<Inquire>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
