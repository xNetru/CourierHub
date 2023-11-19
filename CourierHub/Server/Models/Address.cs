using System;
using System.Collections.Generic;

namespace CourierHub.Server.Models;

public partial class Address
{
    public int Id { get; set; }

    public string Street { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string Flat { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public virtual ICollection<Client> Clients { get; } = new List<Client>();

    public virtual ICollection<Inquire> InquireDestinations { get; } = new List<Inquire>();

    public virtual ICollection<Inquire> InquireSources { get; } = new List<Inquire>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
