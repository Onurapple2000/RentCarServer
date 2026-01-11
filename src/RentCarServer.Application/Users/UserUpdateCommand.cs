using FluentValidation;
using GenericRepository;
using MediatR;
using RentCarServer.Application.Behaviors;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.User;
using RentCarServer.Domain.User.ValueObject;
using TS.Result;

namespace RentCarServer.Application.Users;

[Permission("user:update")]
public sealed record UserUpdareCommand(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    Guid? BranchId,
    Guid RoleId,
    bool IsActive) : IRequest<Result<string>>;

public sealed class UserUpdareCommandValidator : AbstractValidator<UserUpdareCommand>
{
    public UserUpdareCommandValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().WithMessage("Geçerli bir ad girin");
        RuleFor(p => p.LastName).NotEmpty().WithMessage("Geçerli bir soyad girin");
        RuleFor(p => p.UserName).NotEmpty().WithMessage("Geçerli bir kullanıcı adı girin");
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("Geçerli bir mail adresi girin")
            .EmailAddress().WithMessage("Geçerli bir mail adresi girin");
    }
}

internal sealed class UserUpdareCommandHandler(
    IUserRepository userRepository,
    IClaimContext claimContext,
    IUnitOfWork unitOfWork) : IRequestHandler<UserUpdareCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserUpdareCommand request, CancellationToken cancellationToken)
    {

        var user = await userRepository.FirstOrDefaultAsync(p => p.Id.Value == claimContext.GetUserId(), cancellationToken);
        if(user is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı");
        }


        if(user.Email.Value != request.Email)
        {
            var emailExists = await userRepository.AnyAsync(p => p.Email.Value == request.Email, cancellationToken);
            if (emailExists)
            {
                return Result<string>.Failure("Bu mail adresi daha önce kullanılmış");
            }
        }

        if(user.UserName.Value != request.UserName)
        {
            var userNameExists = await userRepository.AnyAsync(p => p.UserName.Value == request.UserName, cancellationToken);
            if (userNameExists)
            {
                return Result<string>.Failure("Bu kullanıcı adı daha önce kullanılmış");
            }
        }
        

       

        var branchId = claimContext.GetBranchId();
        if (request.BranchId is not null)
        {
            branchId = request.BranchId.Value;
        }
        FirstName firstName = new(request.FirstName);
        LastName lastName = new(request.LastName);
        Email email = new(request.Email);
        UserName userName = new(request.UserName);
        IdentityId branchIdRecord = new(branchId);
        IdentityId roleId = new(request.RoleId);
        
        user.setFirstName(firstName);
        user.setLastName(lastName);
        user.setEmail(email);
        user.setFullName();
        user.setUserName(userName);
        user.SetBranchId(branchIdRecord);
        user.SetRoleId(roleId);
        user.SetStatus(request.IsActive);


        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Kullanıcı başarıyla oluşturuldu";
    }
}