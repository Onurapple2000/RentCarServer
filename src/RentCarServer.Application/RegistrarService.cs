using FluentValidation;
using RentCarServer.Application.Behaviors;

namespace RentCarServer.Application;

public static class RegistrarService
{
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        // Add application services here

        services.AddMediatR(cfr =>
        {
            cfr.RegisterServicesFromAssembly(typeof(RegistrarService).Assembly);
            cfr.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfr.AddOpenBehavior(typeof(PermissionBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(RegistrarService).Assembly);

        return services;
    }
}
