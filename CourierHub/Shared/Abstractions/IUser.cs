namespace CourierHub.Shared.Abstractions {
    public interface IUser {
        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
