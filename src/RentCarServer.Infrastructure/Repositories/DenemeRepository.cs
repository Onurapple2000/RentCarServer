using GenericRepository;
using RentCarServer.Domain.Denemeler;
using RentCarServer.Infrastructure.Context;

namespace RentCarServer.Infrastructure.Repository;

internal sealed class DenemeRepository : Repository<Deneme, ApplicationDbContext>, IDenemeRepository
{
    public DenemeRepository(ApplicationDbContext context) : base(context)
    {
    }
}
