using GenericRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentCarServer.Application.Services;
using RentCarServer.Domain.Branches;
using RentCarServer.Domain.LoginTokens;
using RentCarServer.Domain.LoginTokens.ValueObjects;
using RentCarServer.Domain.Roles;
using RentCarServer.Domain.User;
using RentCarServer.Infrastructure.Context;
using RentCarServer.Infrastructure.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace RentCarServer.Infrastructure.Services;

public sealed class JwtProvider(
    ILoginTokenRepository loginTokenRepository,
    IUnitOfWork unitOfWork,
    IRoleRepository roleRepository,
    IBranchRepository branchRepository,
    IOptions<JwtOptions> options) : IJwtProvider
{
    public async Task<string> CreateTokenAsync(User user, CancellationToken cancellationToken = default)
    {
    var role = await roleRepository.FirstOrDefaultAsync(p=> p.Id == user.RoleId, cancellationToken);
        var branch = await branchRepository.FirstOrDefaultAsync(p => p.Id == user.BranchId, cancellationToken);
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim("fullName", user.FirstName.Value + ' ' + user.LastName.Value),
            new Claim("fullNameWithEmail", user.FullName.Value),
            new Claim("email", user.Email.Value),
            new Claim("role", role?.Name.Value ?? string.Empty),
            new Claim("permissions", role is null ? "" : JsonSerializer.Serialize(role.Permissions.Select(s=>s.Value.ToArray()))),
            new Claim("branch", branch?.Name.Value ?? string.Empty),
            new Claim("branchId", branch?.Id ?? string.Empty)
        };

       
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(options.Value.SecretKey));
        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);

        var expires = DateTime.Now.AddDays(1);
        JwtSecurityToken securityToken = new(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            claims: claims,
            notBefore: DateTime.Now,
            expires: expires,
            signingCredentials: signingCredentials);

        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(securityToken);

        Token newToken = new(token);
        ExpiresDate expiresDate = new(expires);
        LoginToken loginToken = new(newToken, user.Id, expiresDate);
        loginTokenRepository.Add(loginToken);

        var loginTokens = await loginTokenRepository
            .Where(p => p.UserId == user.Id && p.IsActive.Value == true)
            .ToListAsync(cancellationToken);
        foreach (var item in loginTokens)
        {
            item.setIsActive(new(false));
        }
        loginTokenRepository.UpdateRange(loginTokens);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return token;
    }
}
