﻿using CourierHub.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
