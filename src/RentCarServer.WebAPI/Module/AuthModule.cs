using MediatR;
using RentCarServer.Application.Auth;
using TS.Result;

namespace RentCarServer.WebAPI.Module;

public static class AuthModule
{

    public static void MapAuth(this IEndpointRouteBuilder builder)
    {

        var app = builder.MapGroup("/auth").RequireRateLimiting("login-fixed");  //program.cs içerisinde yazdığımız login için dakikada 5 den fazla istek gönderilemesin RateLimmeter ı

        app.MapPost("/login", async (LoginCommand request, ISender sender, CancellationToken cancellationToken) =>
        {
            var res = await sender.Send(request, cancellationToken);
            return res.IsSuccessful
                ? Results.Ok(res)
                : Results.Unauthorized();
        }).Produces<Result<string>>(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status401Unauthorized)
          .WithName("Login")
          .WithTags("Auth");


        app.MapPost("/forgot-password/{email}", async (string email, ISender sender, CancellationToken cancellationToken) =>
        {
            var res = await sender.Send(new ForgatPasswordCommand(email), cancellationToken);
            return res.IsSuccessful
                ? Results.Ok(res)
                : Results.Unauthorized();
        }).Produces<Result<string>>(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status401Unauthorized)
          .WithName("Forgot-password")
          .WithTags("Auth");

    }

}
