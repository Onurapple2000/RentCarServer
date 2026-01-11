using MediatR;
using RentCarServer.Application.Auth;
using TS.Result;

namespace RentCarServer.WebAPI.Module;

public static class AuthModule
{

    public static void MapAuth(this IEndpointRouteBuilder builder)
    {

        var app = builder.MapGroup("/auth").RequireRateLimiting("login-fixed").WithTags("Auth"); ;  //program.cs içerisinde yazdığımız login için dakikada 5 den fazla istek gönderilemesin RateLimmeter ı

        app.MapPost("/login", async (LoginCommand request, ISender sender, CancellationToken cancellationToken) =>
        {
            var res = await sender.Send(request, cancellationToken);
            return res.IsSuccessful
                ? Results.Ok(res)
                : Results.Unauthorized();
        }).Produces<Result<LoginCommandResponse>>();// buraya özel rateLimiter yazmadığımız için yukarıdaki genel rate limmeter geçerli olur.



        app.MapPost("/login-with-tfa", async (LoginWithTFACommand request, ISender sender, CancellationToken cancellationToken) =>
        {
            var res = await sender.Send(request, cancellationToken);
            return res.IsSuccessful
                ? Results.Ok(res)
                : Results.InternalServerError(res);
        }).Produces<Result<LoginCommandResponse>>();// buraya özel rateLimiter yazmadığımız için yukarıdaki genel rate limmeter geçerli olur.



        app.MapPost("/forgot-password/{email}", async (string email, ISender sender, CancellationToken cancellationToken) =>
        {
            var res = await sender.Send(new ForgatPasswordCommand(email), cancellationToken);
            return res.IsSuccessful
                ? Results.Ok(res)
                : Results.BadRequest(res);
        }).Produces<Result<string>>()
        .RequireRateLimiting("forgot-password-fixed"); // burada bu path e özel rate limmeter uyguladık. bunu yazmasak yukarıdaki genel rate limeter geçerli olurdu.



        app.MapPost("/reset-password/", async (ResetPasswordCommand request, ISender sender, CancellationToken cancellationToken) =>
        {
            var res = await sender.Send(request, cancellationToken);
            return res.IsSuccessful
                ? Results.Ok(res)
                : Results.BadRequest(res);
        }).Produces<Result<string>>(StatusCodes.Status200OK)
       .RequireRateLimiting("reset-password-fixed") // burada bu path e özel rate limmeter uyguladık. bunu yazmasak yukarıdaki genel rate limeter geçerli olurdu.
         .Produces<Result<string>>();


        app.MapGet("/check-forgot-password-code/{forgotPasswordCode}", async (Guid forgotPasswordCode, ISender sender, CancellationToken cancellationToken) =>
        {
            var res = await sender.Send(new CheckForgotPasswordCodeCommand(forgotPasswordCode), cancellationToken);
            return res.IsSuccessful
                ? Results.Ok(res)
                : Results.BadRequest(res);
        }).Produces<Result<string>>(StatusCodes.Status200OK)
       .RequireRateLimiting("check-forgot-password-code-fixed") // burada bu path e özel rate limmeter uyguladık. bunu yazmasak yukarıdaki genel rate limeter geçerli olurdu.
         .Produces<Result<string>>();

    }

}
