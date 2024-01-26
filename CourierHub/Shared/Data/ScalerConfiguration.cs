using CourierHub.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourierHub.Shared.Data;
public class ScalerConfiguration : IEntityTypeConfiguration<Scaler> {
    public void Configure(EntityTypeBuilder<Scaler> builder) {
        builder.ToTable("Scaler");

        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Company).HasColumnType("money");
        builder.Property(e => e.Depth).HasColumnType("money");
        builder.Property(e => e.Distance).HasColumnType("money");
        builder.Property(e => e.Fee).HasColumnType("money");
        builder.Property(e => e.Length).HasColumnType("money");
        builder.Property(e => e.Mass).HasColumnType("money");
        builder.Property(e => e.Name).HasMaxLength(50);
        builder.Property(e => e.Priority).HasColumnType("money");
        builder.Property(e => e.Time).HasColumnType("money");
        builder.Property(e => e.Weekend).HasColumnType("money");
        builder.Property(e => e.Width).HasColumnType("money");

        // TODO: add scaler
    }
}
