﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierHub.Shared.Models;

public partial class Scaler {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal? Depth { get; set; }

    public decimal? Width { get; set; }

    public decimal? Length { get; set; }

    public decimal? Mass { get; set; }

    public decimal? Distance { get; set; }

    public decimal? Time { get; set; }

    public decimal? Company { get; set; }

    public decimal? Weekend { get; set; }

    public decimal? Priority { get; set; }

    public decimal? Fee { get; set; }

    public float? Tax { get; set; }
}
