namespace AutoFusion.Application.Interfaces;

public interface IGenericService<T, TKey> where T : class
{
    Task<T?> GetByIdAsync(TKey id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(TKey id);
}
