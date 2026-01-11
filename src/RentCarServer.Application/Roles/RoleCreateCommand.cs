using FluentValidation;
using GenericRepository;
using MediatR;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.LoginTokens.ValueObjects;
using RentCarServer.Domain.Roles;
using RentCarServer.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using TS.Result;

namespace RentCarServer.Application.Roles;

[Permission("role.create")]
public sealed record RoleCreateCommand(string Name, bool IsActive): IRequest<Result<string>>;

public sealed class RoleCreateCommandValidator : AbstractValidator<RoleCreateCommand>
{
    public RoleCreateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name cannot be empty.")
            .MaximumLength(100).WithMessage("Role name cannot exceed 100 characters.");
        RuleFor(x => x.IsActive)
            .NotNull().WithMessage("IsActive must be specified.");
    }
}

internal sealed class  RoleCreateCommandHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork) : IRequestHandler<RoleCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
    {
        var nameExists = await roleRepository.AnyAsync(r => r.Name.Value == request.Name, cancellationToken);

        if(nameExists)
        {
            return Result<string>.Failure($"'{request.Name}' rol adı daha önce tanımlanmıştır.");
        }

        Name name = new(request.Name);
        Role role = new(name, request.IsActive);
        roleRepository.Add(role);
        await unitOfWork.SaveChangesAsync(cancellationToken);


        return "Rol başarıyla oluşturuldu.";

    }

    
}

