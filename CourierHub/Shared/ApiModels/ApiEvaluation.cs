using CourierHub.Shared.Models;

namespace CourierHub.Shared.ApiModels;

public class ApiEvaluation {
    public DateTime Datetime { get; set; }

    public string? RejectionReason { get; set; }

    public static explicit operator Evaluation(ApiEvaluation evaluation) {
        return new Evaluation {
            Datetime = evaluation.Datetime,
            RejectionReason = evaluation.RejectionReason
        };
    }

    public static explicit operator ApiEvaluation(Evaluation evaluation) {
        return new ApiEvaluation {
            Datetime = evaluation.Datetime,
            RejectionReason = evaluation.RejectionReason
        };
    }
}
