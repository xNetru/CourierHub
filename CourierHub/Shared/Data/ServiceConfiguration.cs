using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourierHub.Shared.Models;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore.Metadata;

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
                ApiKey = "?",
                IsIntegrated = true
            },
            new Service {
                Id = 2,
                Name = "MiNI.Courier.API",
                BaseAddress = "?",
                Statute = "TBD",
                ApiKey = "team2d;EAAA50B8-90CB-436E-9864-4BC75B56F3BE",
                IsIntegrated = true
            });

        // TODO: add missing values and services
    }
}
