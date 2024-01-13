using CourierHub.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.ApiModels;

public class ApiCourier {
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Imię jest wymagane")]
    [MaxLength(50)]
    [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Imię musi się zaczynać od duzej litery")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    [MaxLength(50)]
    [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Nazwisko musi się zaczynać od duzej litery")]
    public string Surname { get; set; } = null!;

    public static explicit operator ApiCourier(User user) {
        return new ApiCourier {
            Email = user.Email,
            Name = user.Name,
            Surname = user.Surname,
        };
    }

    public static explicit operator User(ApiCourier courier) {
        return new User {
            Email = courier.Email,
            Name = courier.Name,
            Surname = courier.Surname,
            Type = 2
        };
    }
}
