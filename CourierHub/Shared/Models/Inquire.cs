﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierHub.Shared.Models;

public partial class Inquire {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int? ClientId { get; set; }

    public int Depth { get; set; }

    public int Width { get; set; }

    public int Length { get; set; }

    public int Mass { get; set; }

    public int SourceId { get; set; }

    public int DestinationId { get; set; }

    public DateTime SourceDate { get; set; } = DateTime.Today;

    public DateTime DestinationDate { get; set; } = DateTime.Today;

    public DateTime Datetime { get; set; }

    public bool IsCompany { get; set; }

    public bool IsWeekend { get; set; }

    public int Priority { get; set; } = -1;

    public string Code { get; set; } = null!;

    public virtual Address Destination { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual Address Source { get; set; } = null!;

    public virtual User? Client { get; set; }
}
