using MediatR;
using Microsoft.EntityFrameworkCore;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Branches;
using TS.Result;

namespace RentCarServer.Application.Branches;

[Permission("branch.view")]
public sealed record BranchGetQuery(
    Guid Id) : IRequest<Result<BranchDto>>;
internal sealed class BaranchFetQueryHandler(
    IBranchRepository branchRepository) : IRequestHandler<BranchGetQuery, Result<BranchDto>>
{
    public async Task<Result<BranchDto>> Handle(BranchGetQuery request, CancellationToken cancellationToken)
    {
        var branch = await branchRepository
            .GetAllWithAudit()
            .Where(i => i.Entity.Id == request.Id)
            .MapTo()
            .FirstOrDefaultAsync();
        if (branch is null)
        {
            return Result<BranchDto>.Failure("Şube bulunamadı.");
        }



        return branch;
    }
}

