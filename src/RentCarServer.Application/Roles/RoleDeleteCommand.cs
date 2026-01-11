using GenericRepository;
using MediatR;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Text;
using TS.Result;

namespace RentCarServer.Application.Roles;

[Permission("role.delete")]
public sealed record RoleDeleteCommand(
    Guid Id) :IRequest<Result<string>>;

//Validator yazmamıza gerek yok çünkü sadece Id alıyoruz ve IdGuid türünde olduğu için ekstra bir doğrulamaya ihtiyaç yok.Guid değilse zaten hata fırlatır.

internal sealed class RoleDeleteCommandHandler(
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork
    ) : IRequestHandler<RoleDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (role is null)
        {
            return Result<string>.Failure("Şube bulunamadı.");
        }
        role.Delete();
        roleRepository.Update(role);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Şube başarıyla silindi.";
    }
}