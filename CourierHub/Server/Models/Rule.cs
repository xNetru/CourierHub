﻿using System;
using System.Collections.Generic;

namespace CourierHub.Server.Models;

public partial class Rule
{
    public int Id { get; set; }

    public int? DepthMax { get; set; }

    public int? WidthMax { get; set; }

    public int? LengthMax { get; set; }

    public int? MassMax { get; set; }

    public float? VelocityMax { get; set; }
}
