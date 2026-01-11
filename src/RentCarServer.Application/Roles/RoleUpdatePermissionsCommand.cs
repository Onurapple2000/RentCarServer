using GenericRepository;
using MediatR;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Text;
using TS.Result;

namespace RentCarServer.Application.Roles;

[Permission("role:update-permissions")]
public sealed record RoleUpdatePermissionsCommand(Guid RoleId, List<string> Permissions): IRequest<Result<string>>;

internal sealed class RoleUpdatePermissionsCommandHandler(IRoleRepository roleRepository, IUnitOfWork unitOfWork) : IRequestHandler<RoleUpdatePermissionsCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleUpdatePermissionsCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.FirstOrDefaultAsync(r => r.Id == request.RoleId, cancellationToken);
        if(role is null)
        {
            return Result<string>.Failure($"Role with ID {request.RoleId} not found.");
        }

        List<Permission> permissions = request.Permissions.Select(p => new Permission(p)).ToList();
        role.SetPermissions(permissions);
        roleRepository.Update(role);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "İşlem başarılı.";
    }
}