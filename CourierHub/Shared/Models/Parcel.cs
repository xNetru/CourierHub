﻿namespace CourierHub.Shared.Models;

public partial class Parcel {
    public int Id { get; set; }

    public DateTime? PickupDatetime { get; set; }

    public DateTime? DeliveryDatetime { get; set; }

    public string? UndeliveredReason { get; set; }

    public int CourierId { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual User Courier { get; set; } = null!;
}
