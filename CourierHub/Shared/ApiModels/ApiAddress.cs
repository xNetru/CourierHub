using CourierHub.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.ApiModels;
public class ApiAddress {
    [Required(ErrorMessage = "Ulica jest wymagana")]
    [MaxLength(50, ErrorMessage = "Max długośc ulicy wynisi 50 znaków")]
    [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłóśźż]+$", ErrorMessage = "Niewłaściwa wielkość liter")]
    public string Street { get; set; } = null!;

    [Required(ErrorMessage = "Numer budynku jest wymagany")]
    [MaxLength(6, ErrorMessage = "Max długośc numeru budynku wynisi 6 znaków")]
    [RegularExpression(@"^[1-9]\d*[A-ZĄĆĘŁŃÓŚŹŻa-ząćęłóśźż]?$", ErrorMessage = "Niewłaściwy format")]
    public string Number { get; set; } = null!;

    [MaxLength(6, ErrorMessage = "Max długośc numeru lokalu wynisi 6 znaków")]
    [RegularExpression(@"^[1-9]+\d*[A-Za-zĄĆĘŁŃÓŚŹŻąćęłóśźż]?$", ErrorMessage = "Niewłaściwy format")]
    public string? Flat { get; set; } = null!;

    [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
    [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Kod pocztowy musi być postaci XX-XXX")]
    public string PostalCode { get; set; } = null!;

    [Required(ErrorMessage = "Nazwa miasta jest wymagana")]
    [MaxLength(50, ErrorMessage = "Max długośc miasta wynisi 50 znaków")]
    [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłóśźż]*$", ErrorMessage = "Niewłaściwa wielkość liter")]
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
