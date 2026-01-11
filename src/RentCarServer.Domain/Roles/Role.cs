using RentCarServer.Domain.Abstractions;
using RentCarServer.Domain.Shared;

namespace RentCarServer.Domain.Roles;

public sealed class Role : Entity, IAggregate //burada IAggregate işaretlemesi yapıyoruz çünkü Role bir aggregate root tur. aslında boş bir interface sadece role üzerinden
                                              //ulaşabildiğimiz bir permission koleksiyonu var. dikkate çekiyoruz permissionlar role a bağlıdır. role olmadan permission anlamlı değildir.
{
    private readonly List<Permission> _permissions = new();
    private Role() { }

    public Role(Name name, bool isActive)
    {
        SetName(name);
        SetStatus(IsActive);
    }
    public Name Name { get; private set; } = default!;
    public IReadOnlyCollection<Permission> Permissions => _permissions;

    #region Behaviors
    public void SetName(Name name)
    {
        Name = name;
    }

    public void SetPermissions(IEnumerable<Permission> permissions)
    {
        _permissions.Clear();
        _permissions.AddRange(permissions);
    }
    #endregion
}

public sealed record Permission(string Value);