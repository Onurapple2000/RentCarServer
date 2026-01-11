using RentCarServer.Domain.Abstractions;

namespace RentCarServer.Domain.User;

public interface IUserRepository : IAuditableRepository<User>
{
}