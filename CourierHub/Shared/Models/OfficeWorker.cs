namespace CourierHub.Shared.Models;

public partial class OfficeWorker {
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public virtual ICollection<Evaluation> Evaluations { get; } = new List<Evaluation>();
}
