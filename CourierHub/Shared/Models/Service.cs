using System;
using System.Collections.Generic;

namespace CourierHub.Shared.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ApiKey { get; set; } = null!;

    public string Statute { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
