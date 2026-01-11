using Microsoft.EntityFrameworkCore;
using RentCarServer.Domain.LoginTokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentCarServer.Infrastructure.Configurations;

internal sealed class LoginTokenConfiguration : IEntityTypeConfiguration<LoginToken>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<LoginToken> builder)
    {
        builder.ToTable("LoginTokens");
        builder.HasKey(i => i.Id);
        builder.OwnsOne(i => i.Token);
        //builder.OwnsOne(i => i.UserId); Bunu yapmıyoruz çünkü userId yi zaten ApplicationDbContext de özel olarak tanımlıyoruz.
        builder.OwnsOne(i => i.ExpiresDate);
        builder.OwnsOne(i => i.IsActive);

    }
}
