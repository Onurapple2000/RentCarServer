using RentCarServer.Application.Services;
using System.Security.Claims;

namespace RentCarServer.Infrastructure.Services;

public sealed class UserContext(
    HttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid GetUserId()
    {
        var httpContext = httpContextAccessor.HttpContext;
        var claims = httpContext?.User.Claims;
        string? userId = claims?.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            throw new InvalidOperationException("Kullanıcı bilgisi bulunamadı.");
        }

        try
        {
            Guid id = Guid.Parse(userId);
            return id;
        }
        catch (Exception)
        {

            throw new Exception(userId + " geçersiz bir GUID formatında.");
        }




    }
}
