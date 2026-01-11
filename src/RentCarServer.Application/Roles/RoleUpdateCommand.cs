using FluentValidation;
using GenericRepository;
using MediatR;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Roles;
using RentCarServer.Domain.Shared;
using TS.Result;


namespace RentCarServer.Application.Roles;

[Permission("role.edit")]
public sealed record UserUpdateCommand(Guid Id, string Name, bool IsActive) : IRequest<Result<string>>;

public sealed class RoleUpdateCommandValidator : AbstractValidator<UserUpdateCommand>
{
    public RoleUpdateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Role ID boş olamaz");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("role adı boş olamaz.")
            .MaximumLength(100).WithMessage("role adı 100 karakterden fazla olamaz");
        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActif boş olamaz");
    }
}


internal sealed class RoleUpdateCommandHandler(
    IRoleRepository roleRepository, IUnitOfWork unitOfWork) : IRequestHandler<UserUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (role is null)
        {
            return Result<string>.Failure("Rol bulunamadı.");
        }

        role.SetName(new Name(request.Name));
        role.SetStatus(request.IsActive);
        roleRepository.Update(role);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Rol başarılı şekilde güncellendi.";

    }
}
