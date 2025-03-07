using System.Linq.Expressions;

namespace SonicStore.Repository.Repository;
public interface IRepository<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<TEntity> GetByIdAsync(int id);
    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
}