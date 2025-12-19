using GenericRepository;
using Microsoft.EntityFrameworkCore;
using RentCarServer.Infrastructure.Context;
using RentCarServer.Infrastructure.Options;
using Scrutor;

namespace RentCarServer.Infrastructure;

public static class ServiceRegistrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.ConfigureOptions<JwtSetupOptions>(); // Aşsğıdaki services.AddAuthentication().AddJwtBearer() fonksiyonunun optionlarını tanıtıyor.
        services.AddAuthentication().AddJwtBearer(); //yukarıda  services.ConfigureOptions<JwtSetupOptions>(); diyerek tanıttığımız setup optionları Authendication a veriyor.
        services.AddAuthorization();

        services.AddHttpContextAccessor();

        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            string connectionString = configuration.GetConnectionString("SqlServer")!;
            opt.UseSqlServer(connectionString);
        });

        services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

        services.Scan(action => action
            .FromAssemblies(typeof(ServiceRegistrar).Assembly)
            .AddClasses(publicOnly: false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());




        return services;
    }
}
