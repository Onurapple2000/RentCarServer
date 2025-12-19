using FluentValidation;
using MediatR;
using RentCarServer.Domain.User;
using TS.Result;

namespace RentCarServer.Application.Auth;

public sealed record ForgatPasswordCommand(
    string Email
    ) : IRequest<Result<string>>;


public sealed class ForgatPasswordCommandValidator : AbstractValidator<ForgatPasswordCommand>
{
    public ForgatPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");
    }
}


internal sealed class ForgatPasswordCommandHandler(IUserRepository userRepository) : IRequestHandler<ForgatPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ForgatPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(p => p.Email.Value == request.Email, cancellationToken);

        if(user is null)
        {
            return Result<string>.Failure("Böyle bir kullanıcı bulunamadı.");
        }

        // Şifre sıfırlama işlemleri burada yapılacak (örneğin, e-posta gönderme vb.)

        return ("Şifre sıfırlama talimatları e-posta adresinize gönderildi.");

    }
}

