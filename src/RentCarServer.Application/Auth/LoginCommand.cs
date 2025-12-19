using FluentValidation;
using MediatR;
using RentCarServer.Application.Services;
using RentCarServer.Domain.User;
using TS.Result;

namespace RentCarServer.Application.Auth;

public sealed record LoginCommand(
    string EmailOrUserName,
    string Password) : IRequest<Result<string>>;


public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.EmailOrUserName)
            .NotEmpty().WithMessage("Kullanıcı adı ya da email boş olamaz.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz.");
    }
}





public sealed class LoginCommandhandler(IUserRepository userRepository, IJwtProvider jwtProvider) : IRequestHandler<LoginCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(p =>
        p.Email.Value == request.EmailOrUserName
        || p.UserName.Value == request.EmailOrUserName, cancellationToken);

        if (user is null)
        {
            return Result<string>.Failure("Kullanıcı adı ya da şifre yanlış.");
        }

        var checkPasssword = user.VerifyPasswordHash(request.Password);

        if (!checkPasssword)
        {
            return Result<string>.Failure("Kullanıcı adı ya da şifre yanlış.");
        }

        var token = jwtProvider.CreateToken(user);

        return token;

    }
}