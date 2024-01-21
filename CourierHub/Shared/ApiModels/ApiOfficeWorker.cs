using CourierHub.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.ApiModels;

public class ApiOfficeWorker {
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Imię jest wymagane")]
    [MaxLength(50)]
    [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Imię musi się zaczynać od duzej litery")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Nazwisko jest wymagane")]
    [MaxLength(50)]
    [RegularExpression(@"^[A-Z][a-z]*$", ErrorMessage = "Nazwisko musi się zaczynać od duzej litery")]
    public string Surname { get; set; } = null!;

    public static explicit operator ApiOfficeWorker(User user) {
        return new ApiOfficeWorker {
            Email = user.Email,
            Name = user.Name,
            Surname = user.Surname,
        };
    }

    public static explicit operator User(ApiOfficeWorker worker) {
        return new User {
            Email = worker.Email,
            Name = worker.Name,
            Surname = worker.Surname,
            Type = 1
        };
    }
}
