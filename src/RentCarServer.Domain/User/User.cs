using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.User.ValueObject;

namespace RentCarServer.Domain.User;

public sealed class User : Entity
{
    public User(FirstName firstName, LastName lastName, Email email, UserName userName, Password password, IdentityId branchId, IdentityId rolId, bool isActive)
    {
        setFirstName(firstName);
        setLastName(lastName);
        setEmail(email);
        setUserName(userName);
        setPassword(password);
        setFullName();
        setIsForgotPasswordCompleted(new(true));
        SetTFAStatus(new(false));
        SetBranchId(branchId);
        SetRoleId(rolId);
        SetStatus(isActive);
    }

    private User()
    {

    }

    public FirstName FirstName { get; private set; } = null!;
    public LastName LastName { get; private set; } = null!;
    public FullName FullName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public UserName UserName { get; set; } = null!;
    public Password Password { get; private set; } = null!;
    public ForgotPasswordCode? ForgotPasswordCode { get; private set; }
    public ForgotPasswordDate? ForgotPasswordDate { get; private set; }
    public IsForgotPasswordCompleted IsForgotPasswordCompleted { get; private set; } = default!;
    public TFAStatus TFAStatus { get; private set; } = default!;
    public TFACode? TFACode { get; private set; } = default!;
    public TFAConfirmCode? TFAConfirmCode { get; private set; } = default!;
    public TFAExpiresDate? TFAExpiresDate { get; private set; } = default!;
    public TFAIsCompleted? TFAIsCompleted { get; private set; } = default!;
    public IdentityId BranchId { get; private set; } = default!;
    public IdentityId RoleId { get; private set; } = default!;

    #region Behaviors

    public bool VerifyPasswordHash(string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(Password.PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(Password.PasswordHash);
    }

    public void CreateForgotPasswordId()
    {
        ForgotPasswordCode = new(Guid.CreateVersion7());
        ForgotPasswordDate = new(DateTimeOffset.Now);
        IsForgotPasswordCompleted = new(false);
    }

    public void setFirstName(FirstName firstName)
    {
        FirstName = firstName;
    }
    public void setLastName(LastName lastName)
    {
        LastName = lastName;
    }
    public void setUserName(UserName userName)
    {
        UserName = userName;
    }
    public void setEmail(Email email)
    {
        Email = email;
    }
    public void setFullName()
    {
        FullName = new(FirstName.Value + " " + LastName.Value + " (" + Email.Value + ")");
    }


    public void setPassword(Password newPassword)
    {
        Password = newPassword;
    }
    public void setIsForgotPasswordCompleted(IsForgotPasswordCompleted isForgotPasswordCompleted)
    {
        IsForgotPasswordCompleted = isForgotPasswordCompleted;
    }

    public void SetTFAStatus(TFAStatus tfastatus)
    {
        TFAStatus = tfastatus;
    }
    public void CreateTFACode()
    {
        var code = Guid.CreateVersion7().ToString();
        var confirCode = Guid.CreateVersion7().ToString();
        var expires = DateTimeOffset.Now.AddMinutes(5);
        TFACode = new(code);
        TFAConfirmCode = new(confirCode);
        TFAExpiresDate = new(expires);
        TFAIsCompleted = new(false);

    }

    public void SetTFACompleted()
    {
        TFAIsCompleted = new(true);
    }

    public void SetBranchId(IdentityId branchId)
    {
        BranchId = branchId;
    }

    public void SetRoleId(IdentityId roleId)
    {
        RoleId = roleId;
    }

    #endregion
}
