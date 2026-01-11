using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.LoginTokens.ValueObjects;

namespace RentCarServer.Domain.LoginTokens;

public sealed class LoginToken
{
    public LoginToken(
        Token token,
        IdentityId userId,
        ExpiresDate expiresDate)
    {
        Id = new(Guid.CreateVersion7());
        setToken(token);
        setUserId(userId);
        setIsActive(new(true));
        setExpiresDate(expiresDate);
    }

    public LoginToken()
    {
    }

    public IdentityId Id { get; private set; } = default!;
    public Token Token { get; set; } = default!;
    public IdentityId UserId { get; set; } = default!;
    public IsActive IsActive { get; set; } = default!;
    public ExpiresDate ExpiresDate { get; set; } = default!;

    #region Behaviors
    public void setIsActive(IsActive isActive)
    {
        IsActive = isActive;
    }
    public void setToken(Token token)
    {
        Token = token;
    }
    public void setUserId(IdentityId userId)
    {
        UserId = userId;
    }
    public void setExpiresDate(ExpiresDate expiresDate)
    {
        ExpiresDate = expiresDate;
    }
    #endregion

}
