using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.ApiModels {
    public class ApiOrder {
        public int ApiInquire { get; set; }

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

        [Phone(ErrorMessage = "Błędny format numeru telefonu")]
        public string ClientPhone { get; set; } = null!;

        public string ClientCompany { get; set; } = null!;

        [ValidateComplexType]
        public ApiAddress ClientAddress { get; set; } = null!;
    }
}
