using RentCarServer.Domain.Roles;
using RentCarServer.Infrastructure.Abstractions;
using RentCarServer.Infrastructure.Context;

namespace RentCarServer.Infrastructure.Repositories;

internal class RoleRepository : AuditableRepository<Role, ApplicationDbContext>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }
}
