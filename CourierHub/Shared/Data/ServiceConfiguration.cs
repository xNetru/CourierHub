using CourierHub.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourierHub.Shared.Data;
public class ServiceConfiguration : IEntityTypeConfiguration<Service> {
    public void Configure(EntityTypeBuilder<Service> builder) {
        builder.ToTable("Service");

        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.ApiKey)
            .HasMaxLength(100)
            .HasColumnName("Api_key");
        builder.Property(e => e.Name).HasMaxLength(50);
        builder.Property(e => e.Statute).HasMaxLength(100);
        builder.Property(e => e.BaseAddress).HasMaxLength(50);

        builder.HasData(
            new Service {
                Id = 1,
                Name = "CourierHub",
                BaseAddress = "https://courierhub-bck-api.azurewebsites.net/",
                Statute = "TBD",
                ApiKey = "?", // TODO: fill API KEY for us
                IsIntegrated = true
            },
            new Service {
                Id = 2,
                Name = "MiNI.Courier.API",
                BaseAddress = "https://mini.currier.api.snet.com.pl/",
                Statute = "TBD",
                ApiKey = "team2d;EAAA50B8-90CB-436E-9864-4BC75B56F3BE",
                IsIntegrated = true
            },
            new Service {
                Id = 3,
                Name = "WeraHubApi",
                BaseAddress = "https://couriercompanyapi.azurewebsites.net/",
                Statute = "TBD",
                ApiKey = "ApiKey.1",
                IsIntegrated = false
            },
            new Service {
                Id = 4,
                Name = "CourierHub-Kacper",
                BaseAddress = "https://courierhub-bck-api.azurewebsites.net/",
                Statute = "TBD",
                ApiKey = "79a31940-2209-4422-93bd-f0ce9067a3c8",
                IsIntegrated = false
            });
    }
}
