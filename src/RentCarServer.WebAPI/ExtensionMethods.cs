using GenericRepository;
using RentCarServer.Domain.User;
using RentCarServer.Domain.User.ValueObject;

namespace RentCarServer.WebAPI;

public static class ExtensionMethods
{
    public static async Task CreateFirstUser(this WebApplication app)
    {
        using var scoped = app.Services.CreateScope();
        var userRepository = scoped.ServiceProvider.GetRequiredService<IUserRepository>();
        var uniOfWork = scoped.ServiceProvider.GetRequiredService<IUnitOfWork>();

        if (!(await userRepository.AnyAsync(p => p.UserName.Value == "admin")))
        {
            FirstName firstName = new FirstName("Admin");
            LastName lastName = new LastName("Admin");
            FullName fullName = new FullName("Admin Admin");
            Email email = new Email("admin@deneme.com");
            UserName username = new UserName("admin");
            Password password = new("admin123");


            var user = new User(
                firstName,
                lastName,
                email,
                username,
                password
                );
            userRepository.Add(user);
            await uniOfWork.SaveChangesAsync();
        }
    }
}
