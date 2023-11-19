namespace CourierHub.Server.Abstractions {
    public interface IUser {
        string Email { get; set; }

        string Name { get; set; }

        string Surname { get; set; }
    }
}
