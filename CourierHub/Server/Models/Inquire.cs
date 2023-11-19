using System;
using System.Collections.Generic;

namespace CourierHub.Server.Models;

public partial class Inquire
{
    public int Id { get; set; }

    public int? ClientId { get; set; }

    public int Depth { get; set; }

    public int Width { get; set; }

    public int Length { get; set; }

    public int Mass { get; set; }

    public int SourceId { get; set; }

    public int DestinationId { get; set; }

    public DateTime SourceDate { get; set; }

    public DateTime DestinationDate { get; set; }

    public DateTime Datetime { get; set; }

    public bool IsCompany { get; set; }

    public bool IsWeekend { get; set; }

    public int Priority { get; set; }

    public virtual Client? Client { get; set; }

    public virtual Address Destination { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual Address Source { get; set; } = null!;
}
