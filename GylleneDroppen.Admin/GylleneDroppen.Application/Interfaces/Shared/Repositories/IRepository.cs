using System.Linq.Expressions;

namespace GylleneDroppen.Application.Interfaces.Shared.Repositories;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);

    Task AddRangeAsync(List<T> entities);

    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);

    Task<List<T>> GetAllAsync();

    Task<T?> GetByIdAsync(Guid id);

    void Remove(T entity);

    void RemoveRange(List<T> entities);

    Task<bool> SaveChangesAsync();

    void Update(T entity);
}