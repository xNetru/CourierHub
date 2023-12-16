using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.Models;

public partial class ClientData
{
    public int Id { get; set; }

    public byte[]? Photo { get; set; }

    [RegularExpression(@"^\d{9}$", ErrorMessage = "Numer telefonu musi mieć 9 cyfr")]
    public string? Phone { get; set; }

    public string? Company { get; set; }

    public int AddressId { get; set; }

    public int SourceAddressId { get; set; }

    public int ClientId { get; set; }

    [ValidateComplexType]
    public virtual Address Address { get; set; } = null!;

    [ValidateComplexType]
    public virtual Address SourceAddress { get; set; } = null!;

    public virtual User Client { get; set; } = null!;
}
