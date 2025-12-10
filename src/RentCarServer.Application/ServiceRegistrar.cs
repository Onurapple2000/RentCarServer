using FluentValidation;
using RentCarServer.Application.Behaviors;

namespace RentCarServer.Application;

public static class ServiceRegistrar
{
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        // Add application services here

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
