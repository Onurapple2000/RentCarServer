using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.User.ValueObject;

namespace RentCarServer.Domain.User;

public sealed class User : Entity
{
    public User(FirstName firstName, LastName lastName, Email email, UserName userName, Password password)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        UserName = userName;
        Password = password;
        FullName = new(FirstName.Value + " " + LastName.Value + " (" + Email.Value + ")");
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
    public bool VerifyPasswordHash(string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(Password.PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(Password.PasswordHash);
    }
}

