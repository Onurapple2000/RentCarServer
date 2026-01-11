using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RentCarServer.Application.Behaviors;
using RentCarServer.Application.Services;

namespace RentCarServer.Application;

public static class ServiceRegistrar
{
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        // Add application services here
        services.AddScoped<PermissionService>();
        services.AddScoped<PermissionCleanerSevice>();
        services.AddMediatR(cfr =>
        {
            cfr.RegisterServicesFromAssembly(typeof(ServiceRegistrar).Assembly);
            cfr.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfr.AddOpenBehavior(typeof(PermissionBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ServiceRegistrar).Assembly);

        return services;
    }
}
