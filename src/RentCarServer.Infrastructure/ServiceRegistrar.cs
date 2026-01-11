using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RentCarServer.Domain.Branches;
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

        services.Configure<MailSettingOption>(configuration.GetSection("MailSettings"));

        using var scoped = services.BuildServiceProvider().CreateScope();
        var mailSettings = scoped.ServiceProvider.GetRequiredService<IOptions<MailSettingOption>>();

        if(string.IsNullOrEmpty(mailSettings.Value.UserId))
        { //bilgisayardaki fake smtp server için
            services.AddFluentEmail(mailSettings.Value.Email).AddSmtpSender(
                mailSettings.Value.Smtp,
                mailSettings.Value.Port);
        } else
        { //gerçek smtp server için
            services.AddFluentEmail(mailSettings.Value.Email).AddSmtpSender(
                mailSettings.Value.Smtp,
                mailSettings.Value.Port,
                mailSettings.Value.UserId,
                mailSettings.Value.Password);

        }

            

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
