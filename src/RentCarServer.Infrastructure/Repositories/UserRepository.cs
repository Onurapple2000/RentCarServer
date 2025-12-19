using GenericRepository;
using RentCarServer.Domain.User;
using RentCarServer.Infrastructure.Context;

namespace RentCarServer.Infrastructure.Repositories;

public class UserRepository : Repository<User, ApplicationDbContext>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {

    }
}
