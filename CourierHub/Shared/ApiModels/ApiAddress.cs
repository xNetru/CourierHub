using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.ApiModels {
    public class ApiAddress {
        [Required(ErrorMessage = "Ulica jest wymagana")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "Numer budynku jest wymagany")]
        public string Number { get; set; } = null!;

        [Required(ErrorMessage = "Numer lokalu jest wymagany")]
        public string? Flat { get; set; } = null!;

        [Required(ErrorMessage = "Kod pocztowy jest wymagany")]
        [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Kod pocztowy musi być postaci XX-XXX")]
        public string PostalCode { get; set; } = null!;

        [Required(ErrorMessage = "Miasto jest wymagane")]
        public string City { get; set; } = null!;
    }
}
