using Microsoft.EntityFrameworkCore;
using SonicStore.Repository.Repository;
using System.Linq.Expressions;

namespace SonicStore.Business.Service;
public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
{
    protected readonly IRepository<TEntity> _repository;

    public BaseService(IRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public async Task AddAsync(TEntity entity)
    {
        await _repository.AddAsync(entity);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await _repository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        await _repository.DeleteAsync(entity);
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _repository.Find(predicate).ToListAsync();
    }

    public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _repository.Find(predicate).FirstOrDefaultAsync();
    }
}
