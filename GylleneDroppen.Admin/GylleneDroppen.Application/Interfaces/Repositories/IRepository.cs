using System.Linq.Expressions;

namespace GylleneDroppen.Application.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    Task AddAsync(T entity);

    Task AddRangeAsync(List<T> entities);

    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);

    Task<List<T>> GetAllAsync();
    
    Task<List<T>> GetAllAsync<TProperty>(Expression<Func<T, TProperty>> includeExpression, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

    Task<T?> GetByIdAsync(Guid id);
    
    Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeExpressions);

    void Remove(T entity);
    
    void Delete(T entity);

    void RemoveRange(List<T> entities);

    Task<bool> SaveChangesAsync();

    void Update(T entity);
}