using FluentValidation;
using GenericRepository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RentCarServer.Domain.LoginTokens;
using RentCarServer.Domain.User;
using RentCarServer.Domain.User.ValueObject;
using TS.Result;

namespace RentCarServer.Application.Auth;

public sealed record ResetPasswordCommand(
    Guid ForgotPasswordCode,
    string NewPassword,
    bool logoutAllDevices
) : IRequest<Result<string>>;

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.ForgotPasswordCode)
            .NotEmpty().WithMessage("ForgotPasswordCode boş olamaz.");
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Yeni şifre boş olamaz.")
            .MinimumLength(6).WithMessage("Yeni şifre en az 8 karakter olmalıdır.");
    }
}

internal sealed class ResetPasswordCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILoginTokenRepository loginTokenRepository
) : IRequestHandler<ResetPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(p => p.ForgotPasswordCode != null &&
        p.ForgotPasswordCode.Value == request.ForgotPasswordCode &&
        p.IsForgotPasswordCompleted.Value == false
        , cancellationToken);
        if (user is null)
        {
            return Result<string>.Failure("Geçersiz veya süresi dolmuş şifre sıfırlama isteği.");
        }

        var fpDate = user.ForgotPasswordDate!.Value.AddDays(1);
        var now = DateTimeOffset.Now;

        if (fpDate <= now)
        {
            return Result<string>.Failure("Şifre sıfırlama değeriniz geçersizdir.");
        }

        Password password = new Password(request.NewPassword);
        user.setPassword(password);
        userRepository.Update(user);
        
        if(request.logoutAllDevices)
        {
              var loginTokens = await loginTokenRepository
            .Where(p => p.UserId == user.Id && p.IsActive.Value == true)
            .ToListAsync(cancellationToken);
        foreach (var item in loginTokens)
        {
            item.setIsActive(new(false));
        }
        loginTokenRepository.UpdateRange(loginTokens);

        }
       

      
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ("Şifreniz başarıyla sıfırlandı. Yeni şifrenizle giriş yapabilirsiniz");
    }
}


