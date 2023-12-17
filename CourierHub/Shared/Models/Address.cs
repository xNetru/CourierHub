<<<<<<< HEAD
<<<<<<< HEAD
﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierHub.Shared.Models;
=======
﻿namespace CourierHub.Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
>>>>>>> 28296336b69f8456147901bf8b138a44ba40ba84
=======
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierHub.Shared.Models;

>>>>>>> 617ad29b7bf85550604dda58be4ccf9a5f4f6de1

public partial class Address {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Ulica jest wymagana")]
    public string Street { get; set; } = null!;

    [Required(ErrorMessage = "Numer budynku jest wymagany")]
    public string Number { get; set; } = null!;

    [Required(ErrorMessage = "Numer lokalu jest wymagany")]
    public string? Flat { get; set; } = null!;

    [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
    [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Kod pocztowy musi być postaci XX-XXX")]
    public string PostalCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public virtual ICollection<ClientData> ClientDatumAddresses { get; } = new List<ClientData>();

    public virtual ICollection<ClientData> ClientDatumSourceAddresses { get; } = new List<ClientData>();

    public virtual ICollection<Inquire> InquireDestinations { get; } = new List<Inquire>();

    public virtual ICollection<Inquire> InquireSources { get; } = new List<Inquire>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
