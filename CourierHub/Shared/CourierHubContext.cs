using CourierHub.Shared.Objects;
using Microsoft.EntityFrameworkCore;

namespace CourierHub.Shared {
    public class CourierHubContext : DbContext {

        public DbSet<Client> Clients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Server=courierhub.database.windows.net;Database=CourierHub;Trusted_Connection=True;");
        }
    }
}