using CourierHub.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.ApiModels;

public class ApiOrder {
    public string Code { get; set; } = null!;

    public decimal Price { get; set; }

    [Required(ErrorMessage = "Email jest wymagany")]
    [EmailAddress(ErrorMessage = "Niepoprawny format maila")]
    public string ClientEmail { get; set; } = null!;

    [Required(ErrorMessage = "Imię jest wymagane")]
    [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Imię musi się zaczynać od duzej litery")]
    public string ClientName { get; set; } = null!;

    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Nazwisko musi się zaczynać od duzej litery")]
    public string ClientSurname { get; set; } = null!;

    [Required(ErrorMessage = "Numer telefonu jest wymagany")]
    [Phone(ErrorMessage = "Błędny format numeru telefonu")]
    public string ClientPhone { get; set; } = null!;

    public string ClientCompany { get; set; } = null!; // chyba niewymagane?

    [ValidateComplexType]
    public ApiAddress ClientAddress { get; set; } = null!;

    public static explicit operator ApiOrder(Order order) {
        return new ApiOrder {
            Code = order.Inquire.Code,
            Price = order.Price,
            ClientEmail = order.ClientEmail,
            ClientName = order.ClientName,
            ClientSurname = order.ClientSurname,
            ClientPhone = order.ClientPhone,
            ClientCompany = order.ClientCompany,
            ClientAddress = (ApiAddress)order.ClientAddress
        };
    }

    public static explicit operator Order(ApiOrder order) {
        return new Order {
            Price = order.Price,
            ClientEmail = order.ClientEmail,
            ClientName = order.ClientName,
            ClientSurname = order.ClientSurname,
            ClientPhone = order.ClientPhone,
            ClientCompany = order.ClientCompany,
            ClientAddress = (Address)order.ClientAddress
        };
    }
}
