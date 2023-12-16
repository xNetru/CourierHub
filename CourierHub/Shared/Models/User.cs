﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.Models;

public abstract partial class User {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public virtual int Type { get; set; }

    public virtual ICollection<ClientData> ClientData { get; } = new List<ClientData>();

    public virtual ICollection<Evaluation> Evaluations { get; } = new List<Evaluation>();

    public virtual ICollection<Inquire> Inquires { get; } = new List<Inquire>();

    public virtual ICollection<Parcel> Parcels { get; } = new List<Parcel>();
}
