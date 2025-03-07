using System.Linq.Expressions;

namespace SonicStore.Business.Service;
public interface IBaseService<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<TEntity> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate);
}