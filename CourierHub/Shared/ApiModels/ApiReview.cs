using CourierHub.Shared.Models;

namespace CourierHub.Shared.ApiModels;

public class ApiReview {
    public int Value { get; set; }

    public string? Description { get; set; }

    public DateTime Datetime { get; set; }

    public static explicit operator Review(ApiReview review) {
        return new Review {
            Value = review.Value,
            Description = review.Description,
            Datetime = review.Datetime
        };
    }

    public static explicit operator ApiReview(Review review) {
        return new ApiReview {
            Value = review.Value,
            Description = review.Description,
            Datetime = review.Datetime
        };
    }
}
