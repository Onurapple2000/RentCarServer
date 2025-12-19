using RentCarServer.Domain.User;

namespace RentCarServer.Application.Services;

public interface IJwtProvider
{
    string CreateToken(User user);
}
