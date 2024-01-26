using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourierHub.Shared.Models;
using System.Reflection.Emit;

namespace CourierHub.Shared.Data;
public class StatusConfiguration : IEntityTypeConfiguration<Status> {
    public void Configure(EntityTypeBuilder<Status> builder) {
        builder.ToTable("Status");
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Name).HasMaxLength(50);
        builder.HasData(
            new Status { Id = 1, Name = "NotConfirmed", IsCancelable = true },
            new Status { Id = 2, Name = "Confirmed", IsCancelable = true },
            new Status { Id = 3, Name = "Cancelled", IsCancelable = false },
            new Status { Id = 4, Name = "Denied", IsCancelable = false },
            new Status { Id = 5, Name = "PickedUp", IsCancelable = false },
            new Status { Id = 6, Name = "Delivered", IsCancelable = false },
            new Status { Id = 7, Name = "CouldNotDeliver", IsCancelable = false }
        );
    }
}
