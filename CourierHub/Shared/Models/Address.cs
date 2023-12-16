﻿using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.Models;

public partial class Address
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Ulica jest wymagana")]
    public string Street { get; set; } = null!;

    [Required(ErrorMessage = "Numer budynku jest wymagany")]
    public string Number { get; set; } = null!;

    [Required(ErrorMessage = "Numer lokalu jest wymagany")]
    public string Flat { get; set; } = null!;

    [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
    [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Kod pocztowy musi być postaci XX-XXX")]
    public string PostalCode { get; set; } = null!;

    public virtual ICollection<ClientData> ClientDatumAddresses { get; } = new List<ClientData>();

    public virtual ICollection<ClientData> ClientDatumSourceAddresses { get; } = new List<ClientData>();

    public virtual ICollection<Inquire> InquireDestinations { get; } = new List<Inquire>();

    public virtual ICollection<Inquire> InquireSources { get; } = new List<Inquire>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
