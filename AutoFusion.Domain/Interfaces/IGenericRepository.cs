using System.Linq.Expressions;

namespace AutoFusion.Domain.Interfaces;

public interface IGenericRepository<T, TKey> where T : class
{
    Task<T?> GetByIdAsync(TKey id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(TKey id);
}
