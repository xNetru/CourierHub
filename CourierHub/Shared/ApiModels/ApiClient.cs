using CourierHub.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.ApiModels;

public class ApiClient {
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Imię jest wymagane")]
    [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Imię musi się zaczynać od duzej litery")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Nazwisko musi się zaczynać od duzej litery")]
    public string Surname { get; set; } = null!;

    public byte[]? Photo { get; set; }

    [Phone]
    public string? Phone { get; set; }

    public string? Company { get; set; }

    [ValidateComplexType]
    public ApiAddress Address { get; set; } = null!;

    [ValidateComplexType]
    public ApiAddress SourceAddress { get; set; } = null!;

    public static explicit operator User(ApiClient client) {
        return new User {
            Email = client.Email,
            Name = client.Name,
            Surname = client.Surname,
            Type = 0
        };
    }

    public static explicit operator ClientData(ApiClient client) {
        return new ClientData {
            Photo = client.Photo,
            Phone = client.Phone,
            Company = client.Company,
            Address = (Address)client.Address,
            SourceAddress = (Address)client.SourceAddress
        };
    }
}
