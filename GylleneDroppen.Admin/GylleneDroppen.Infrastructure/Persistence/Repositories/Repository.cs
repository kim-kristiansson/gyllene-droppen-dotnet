using System.Linq.Expressions;
using GylleneDroppen.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GylleneDroppen.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> DbSet;

    protected Repository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        DbSet = _context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(List<T> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.Where(predicate).ToListAsync();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }
    
    public async Task<List<T>> GetAllAsync<TProperty>(Expression<Func<T, TProperty>> includeExpression, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
    {
        IQueryable<T> query = DbSet.Include(includeExpression);
        
        if (orderBy != null)
        {
            query = orderBy(query);
        }
        
        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet.FindAsync(id);
    }
    
    public async Task<T?> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includeExpressions)
    {
        var query = DbSet.AsQueryable();
        
        foreach (var includeExpression in includeExpressions)
        {
            query = query.Include(includeExpression);
        }
        
        return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
    }

    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }
    
    public void Delete(T entity)
    {
        DbSet.Remove(entity);
    }

    public void RemoveRange(List<T> entities)
    {
        DbSet.RemoveRange(entities);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }
}