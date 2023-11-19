using System;
using System.Collections.Generic;

namespace CourierHub.Server.Models;

public partial class Client
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public byte[]? Photo { get; set; }

    public string? Phone { get; set; }

    public string? Company { get; set; }

    public int? AddressId { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Inquire> Inquires { get; } = new List<Inquire>();
}
