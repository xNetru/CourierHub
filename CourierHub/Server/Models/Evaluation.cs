using System;
using System.Collections.Generic;

namespace CourierHub.Server.Models;

public partial class Evaluation
{
    public int Id { get; set; }

    public DateTime Datetime { get; set; }

    public string? RejectionReason { get; set; }

    public int OfficeWorkerId { get; set; }

    public virtual OfficeWorker OfficeWorker { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
