namespace CourierHub.Shared.Models;

public partial class Evaluation {
    public int Id { get; set; }

    public DateTime Datetime { get; set; }

    public string? RejectionReason { get; set; }

    public int OfficeWorkerId { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual User OfficeWorker { get; set; } = null!;
}
