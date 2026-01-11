using RentCarServer.Domain.User;
using RentCarServer.Infrastructure.Abstractions;
using RentCarServer.Infrastructure.Context;

namespace RentCarServer.Infrastructure.Repositories;

internal sealed class UserRepository : AuditableRepository<User, ApplicationDbContext>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}