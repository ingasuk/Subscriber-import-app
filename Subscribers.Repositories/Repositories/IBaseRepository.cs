namespace Subscribers.Repositories.Repositories;
public interface IBaseRepository<TEntity> where TEntity : class, IBaseEntity
{
    IQueryable<TEntity> GetAll();
    Task<TEntity> Insert(TEntity entity);
    Task<TEntity> Update(TEntity entity);
}
