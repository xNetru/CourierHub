﻿using System;
using System.Collections.Generic;

namespace CourierHub.Server.Models;

public partial class Review
{
    public int Id { get; set; }

    public int Value { get; set; }

    public string? Description { get; set; }

    public DateTime Datetime { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
