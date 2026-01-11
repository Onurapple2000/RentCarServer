using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentCarServer.Domain.Denemeler;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentCarServer.Infrastructure.Configurations;

internal sealed class DenemeConfiguration : IEntityTypeConfiguration<Deneme>
{
    public void Configure(EntityTypeBuilder<Deneme> builder)
    {
        builder.ToTable("Denemeler");
        builder.HasKey(i => i.DenemeId);
        builder.OwnsOne(i=> i.DenemeStr1, b => b.Property(p => p.Value).HasColumnName("DenemeStr1").HasMaxLength(100).IsRequired());
        builder.OwnsOne(i=> i.DenemeStr2, b => b.Property(p => p.Value).HasColumnName("DenemeStr2").HasMaxLength(50));
        builder.OwnsOne(i=> i.DenemeInt1, b => b.Property(p => p.Value).HasColumnName("DenemeInt1").IsRequired());
        builder.OwnsOne(i=> i.DenemeInt2, b => b.Property(p => p.Value).HasColumnName("DenemeInt2"));
        builder.OwnsOne(i=> i.DenemeBool1, b => b.Property(p => p.Value).HasColumnName("DenemeBool1").IsRequired());
        builder.OwnsOne(i=> i.DenemeBool2, b => b.Property(p => p.Value).HasColumnName("DenemeBool2"));
    }
}
