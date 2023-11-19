﻿using System;
using System.Collections.Generic;

namespace CourierHub.Server.Models;

public partial class Courier
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public virtual ICollection<Parcel> Parcels { get; } = new List<Parcel>();
}
