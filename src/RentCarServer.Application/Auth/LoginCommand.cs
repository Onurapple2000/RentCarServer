using FluentValidation;
using GenericRepository;
using MediatR;
using RentCarServer.Application.Services;
using RentCarServer.Domain.User;
using TS.Result;

namespace RentCarServer.Application.Auth;

public sealed record LoginCommand(
    string EmailOrUserName,
    string Password) : IRequest<Result<LoginCommandResponse>>;



public sealed record LoginCommandResponse
{
    public string? Token { get; set; } = null!;
    public string? TFACode { get; set; } = null!;

}


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





public sealed class LoginCommandHandler(IUserRepository userRepository,
    IJwtProvider jwtProvider,
    IMailService mailService,
    IUnitOfWork unitOfWork) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(p =>
        p.Email.Value == request.EmailOrUserName
        || p.UserName.Value == request.EmailOrUserName, cancellationToken);

        if (user is null)
        {
            return Result<LoginCommandResponse>.Failure("Kullanıcı adı ya da şifre yanlış.");
        }

        var checkPasssword = user.VerifyPasswordHash(request.Password);

        if (!checkPasssword)
        {
            return Result<LoginCommandResponse>.Failure("Kullanıcı adı ya da şifre yanlış.");
        }


        if (!user.TFAStatus.Value)
        {
            var token = await jwtProvider.CreateTokenAsync(user, cancellationToken);
            var response = new LoginCommandResponse
            {
                Token = token,
                TFACode = null
            };
            return response;
        }
        else
        {
            user.CreateTFACode();

            userRepository.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            string to = user.Email.Value;
            string subject = "RentCarServer İki Adımlı Doğrulama Kodu";
            string body = $"İki adımlı doğrulama kodunuz: <h4> {user.TFAConfirmCode!.Value} </h4>. Lütfen bu kodu kullanarak giriş yapın. Bu Kod 5 dk geçerlidir.";
            await mailService.SendAsync(to, subject, body, cancellationToken);

            var response = new LoginCommandResponse
            {
                Token = null,
                TFACode = user.TFACode?.Value
            };
            return response;

        }



    }
}