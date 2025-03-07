using Microsoft.EntityFrameworkCore;
using SonicStore.Repository.Entity;
using System.Linq.Expressions;

namespace SonicStore.Repository.Repository;
public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly SonicStoreContext _context;
    protected readonly DbSet<TEntity> _entities;

    public BaseRepository(SonicStoreContext context)
    {
        _context = context;
        _entities = context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _entities.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _entities.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _entities.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _entities.FindAsync(id);
    }

    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return _entities.Where(predicate);
    }
}