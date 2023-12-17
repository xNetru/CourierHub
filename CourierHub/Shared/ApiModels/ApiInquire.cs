using CourierHub.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace CourierHub.Shared.ApiModels {
    public class ApiInquire {
        [Required(ErrorMessage = "Głębokość paczki jest wymagana")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Głębokość musi być liczbą bez wiodących 0")]
        public int Depth { get; set; }

        [Required(ErrorMessage = "Szerokość paczki jest wymagana")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Szerokość musi być liczbą bez wiodących 0")]
        public int Width { get; set; }

        [Required(ErrorMessage = "Długość paczki jest wymagana")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Długość musi być liczbą bez wiodących 0")]
        public int Length { get; set; }

        [Required(ErrorMessage = "Masa paczki jest wymagana")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Masa musi być liczbą bez wiodących 0")]
        public int Mass { get; set; }

        [Comparison("DestinationDate", ComparisonType.LessThanOrEqualTo, ErrorMessage = "Data wysyłki nie może być większa niż data dostawy")]
        public DateTime SourceDate { get; set; } = DateTime.Today;

        [Comparison("SourceDate", ComparisonType.GreaterThanOrEqualTo, ErrorMessage = "Data dostawy nie może być mniejsza niż data wysyłki")]
        public DateTime DestinationDate { get; set; } = DateTime.Today;

        public DateTime Datetime { get; set; }

        public bool IsCompany { get; set; }

        public bool IsWeekend { get; set; }

        [Range(0, 2, ErrorMessage = "Należy ustalić priorytet")]
        public int Priority { get; set; } = -1;

        [ValidateComplexType]
        public virtual ApiAddress Destination { get; set; } = null!;

        [ValidateComplexType]
        public virtual ApiAddress Source { get; set; } = null!;
    }
}
