using RentCarServer.Domain.User;

namespace RentCarServer.Application.Services;

public interface IJwtProvider
{
    Task<string> CreateTokenAsync(User user, CancellationToken cancellationToken = default);
}
