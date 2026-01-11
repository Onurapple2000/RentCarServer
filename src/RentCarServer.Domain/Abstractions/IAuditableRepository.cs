using GenericRepository;
using UserEntity = RentCarServer.Domain.User.User;
namespace RentCarServer.Domain.Abstractions;

public interface IAuditableRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
{
    IQueryable<EntityWithAuditDto<TEntity>> GetAllWithAudit();
}

public sealed class EntityWithAuditDto<TEntity> where TEntity : Entity
{
    public TEntity Entity { get; set; } = default!;
    public UserEntity CreatedUser { get; set; } = default!;
    public UserEntity? UpdatedUser { get; set; }

}