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
public class RuleConfiguration : IEntityTypeConfiguration<Rule> {
    public void Configure(EntityTypeBuilder<Rule> builder) {
        builder.ToTable("Rule");

        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.DepthMax).HasColumnName("Depth_max");
        builder.Property(e => e.LengthMax).HasColumnName("Length_max");
        builder.Property(e => e.MassMax).HasColumnName("Mass_max");
        builder.Property(e => e.VelocityMax).HasColumnName("Velocity_max");
        builder.Property(e => e.WidthMax).HasColumnName("Width_max");

        // TODO: add rule
    }
}
