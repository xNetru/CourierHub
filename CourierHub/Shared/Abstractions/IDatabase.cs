namespace CourierHub.Server.Abstractions {
    public interface IDatabase {
        Task<bool> checkUser(string email);

        Task addUser(IUser user);
    }
}
