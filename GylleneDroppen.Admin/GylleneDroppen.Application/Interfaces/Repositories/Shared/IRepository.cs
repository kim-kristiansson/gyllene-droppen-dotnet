using System.Linq.Expressions;

namespace GylleneDroppen.Application.Interfaces.Repositories.Shared;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task AddRangeAsync(List<T> entities);
    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(List<T> entities);
    Task<bool> SaveChangesAsync();
}