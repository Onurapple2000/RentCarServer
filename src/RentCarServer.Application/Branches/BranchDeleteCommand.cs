using GenericRepository;
using MediatR;
using RentCarServer.Application.Behaviors;
using RentCarServer.Domain.Branches;
using TS.Result;

namespace RentCarServer.Application.Branches;

[Permission("branch.delete")]
public sealed record BranchDeleteCommand(
    Guid Id) : IRequest<Result<string>>;


//Validator yazmamıza gerek yok çünkü sadece Id alıyoruz ve IdGuid türünde olduğu için ekstra bir doğrulamaya ihtiyaç yok.Guid değilse zaten hata fırlatır.

internal sealed class BranchDeleteCommandHandler(
    IBranchRepository branchRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<BranchDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(BranchDeleteCommand request, CancellationToken cancellationToken)
    {
        var branch = await branchRepository.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
        if (branch is null)
        {
            return Result<string>.Failure("Şube bulunamadı.");
        }
        branch.Delete();
        branchRepository.Update(branch);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Şube başarılı şekilde silindi.";
    }
}


