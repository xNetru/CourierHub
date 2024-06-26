﻿using CourierHub.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.ApiModels;

public class ApiOrder {
    public string Code { get; set; } = null!;

    public decimal Price { get; set; }

    [Required(ErrorMessage = "Email jest wymagany")]
    [MaxLength(50)]
    [EmailAddress(ErrorMessage = "Niepoprawny format maila")]
    public string ClientEmail { get; set; } = null!;

    [Required(ErrorMessage = "Imię jest wymagane")]
    [MaxLength(50)]
    [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłóśźż]*$", ErrorMessage = "Imię musi się zaczynać od duzej litery")]
    public string ClientName { get; set; } = null!;

    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    [MaxLength(50)]
    [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻ][a-ząćęłóśźż]*$", ErrorMessage = "Nazwisko musi się zaczynać od duzej litery")]
    public string ClientSurname { get; set; } = null!;

    [Required(ErrorMessage = "Numer telefonu jest wymagany")]
    [MaxLength(12)]
    [RegularExpression(@"^\d*$", ErrorMessage = "Błędny format numeru telefonu")]
    public string ClientPhone { get; set; } = null!;

    [MaxLength(50)]
    [RegularExpression(@"^[A-ZĄĆĘŁŃÓŚŹŻa-ząćęłóśźż0-9]*$", ErrorMessage = "Błędny format nazwy firmy")]
    public string? ClientCompany { get; set; }

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
