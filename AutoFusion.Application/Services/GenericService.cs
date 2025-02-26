using AutoFusion.Application.Interfaces;
using AutoFusion.Domain.Interfaces;

namespace AutoFusion.Application.Services;

public class GenericService<T, TKey> : IGenericService<T, TKey> where T : class
{
    protected readonly IGenericRepository<T, TKey> _repository;

    public GenericService(IGenericRepository<T, TKey> repository)
    {
        _repository = repository;
    }

    public virtual async Task<T?> GetByIdAsync(TKey id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _repository.AddAsync(entity);
    }

    public virtual async Task UpdateAsync(T entity)
    {
        await _repository.UpdateAsync(entity);
    }

    public virtual async Task DeleteAsync(TKey id)
    {
        await _repository.DeleteAsync(id);
    }
}
