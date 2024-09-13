using Microsoft.EntityFrameworkCore;
using Subscribers.Repositories.Context;
using Subscribers.Repositories.Repositories;

namespace Subscribers.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    private readonly SubscriberDatabaseContext _databaseContext;
    private readonly DbSet<TEntity> _dbSet;

    public BaseRepository(SubscriberDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
        _dbSet = _databaseContext.Set<TEntity>();
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbSet;
    }

    public virtual async Task<TEntity> Insert(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await _databaseContext.SaveChangesAsync();

        return entity;
    }

    public virtual async Task<TEntity> Update(TEntity entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;
        _dbSet.Update(entity);
        await _databaseContext.SaveChangesAsync();

        return entity;
    }
}
