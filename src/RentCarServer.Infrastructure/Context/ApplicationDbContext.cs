using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Denemeler;
using RentCarServer.Domain.LoginTokens;
using RentCarServer.Domain.Roles;
using RentCarServer.Domain.User;
using System.Security.Claims;

namespace RentCarServer.Infrastructure.Context;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    //public DbSet<User> Users { get; set; } bunlara gerek kalmadı çünkü configuration dostasında ToTable ile tabloyu gösterdik
    //public DbSet<LoginToken> LoginTokens { get; set; }
    //public DbSet<Branch> Branchs { get; set; }
    //public DbSet<Deneme> Denemeler { get; set; }
    //public DbSet<Role> Roles { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly); // benim yadığım configurationsları otomatik olarak bulup ekliyor. configurations klasörü altında Her Entity için var
        modelBuilder.ApplyGlobalFilters(); // burada global filtreleri uyguluyoruz soft delete için. EF bunu otomatik olarak GetAll içerisine ekliyor.
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<IdentityId>().HaveConversion<IdentityIdValueConverter>(); //burada userId için özel olarak işaretleme yappıyoruz bu şekilde tekrar konfigurasyon yapmaya gerek kalmıyor.böylece tüm IdentityId tipleri için bu konvertör kullanılır. EF db de Guid olarak saklanacağını bilir.
        configurationBuilder.Properties<decimal>().HaveColumnType("decimal(18,2)");
        configurationBuilder.Properties<string>().HaveColumnType("nvarchar(MAX)");
        base.ConfigureConventions(configurationBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();

        HttpContextAccessor httpContextAccessor = new();
        string? userIdString =
        httpContextAccessor
        .HttpContext?
        .User
        .Claims
        .FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?
        .Value;

        if (userIdString is null)
        {
            return base.SaveChangesAsync(cancellationToken);
        }


        Guid userId = Guid.Parse(userIdString);
        IdentityId identityId = new(userId);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreatedAt)
                    .CurrentValue = DateTimeOffset.Now;
                entry.Property(p => p.CreatedBy)
                    .CurrentValue = identityId;
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entry.Property(p => p.DeletedAt)
                    .CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.DeletedBy)
                    .CurrentValue = identityId;
                }
                else
                {
                    entry.Property(p => p.UpdatedAt)
                        .CurrentValue = DateTimeOffset.Now;
                    entry.Property(p => p.UpdatedBy)
                    .CurrentValue = identityId;
                }
            }

            if (entry.State == EntityState.Deleted)
            {
                throw new ArgumentException("Db'den direkt silme işlemi yapamazsınız");
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}
internal sealed class IdentityIdValueConverter : ValueConverter<IdentityId, Guid>
{
    public IdentityIdValueConverter() : base(m => m.Value, m => new IdentityId(m)) { }
}