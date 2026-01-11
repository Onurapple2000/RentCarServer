using GenericRepository;
using Microsoft.EntityFrameworkCore;
using RentCarServer.Domain.LoginTokens;
using RentCarServer.Infrastructure.Context;

namespace RentCarServer.Infrastructure.Repository;

internal sealed class LoginTokenRepository : Repository<LoginToken, ApplicationDbContext>, ILoginTokenRepository
{
    public LoginTokenRepository(ApplicationDbContext context) : base(context)
    {
    }
}
