using CourierHub.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.ApiModels;

public class ApiAddress {
    [Required(ErrorMessage = "Ulica jest wymagana")]
    public string Street { get; set; } = null!;

    [Required(ErrorMessage = "Numer budynku jest wymagany")]
    public string Number { get; set; } = null!;

    public string? Flat { get; set; } = null!;

    [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
    [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Kod pocztowy musi być postaci XX-XXX")]
    public string PostalCode { get; set; } = null!;

    [Required(ErrorMessage = "Nazwa miasta jest wymagana")]
    public string City { get; set; } = null!;

    public static explicit operator ApiAddress(Address address) {
        return new ApiAddress {
            Street = address.Street,
            Number = address.Number,
            Flat = address.Flat,
            PostalCode = address.PostalCode,
            City = address.City,
        };
    }

    public static explicit operator Address(ApiAddress address) {
        return new Address {
            Street = address.Street,
            Number = address.Number,
            Flat = address.Flat,
            PostalCode = address.PostalCode,
            City = address.City,
        };
    }
}
