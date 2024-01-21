using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierHub.Shared.Models;

public partial class ClientData {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public byte[]? Photo { get; set; }

    public string? Phone { get; set; }

    public string? Company { get; set; }

    public int AddressId { get; set; }

    public int SourceAddressId { get; set; }

    public int ClientId { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual Address SourceAddress { get; set; } = null!;

    public virtual User? Client { get; set; } = null!;
}
