using System.Linq.Expressions;

namespace InventoryManagementSystem.Core.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Find entities with a predicate and optional ordering (executed on the database).
    /// </summary>
    Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

    Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize);
    Task<int> CountAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);

    /// <summary>
    /// Commit all pending changes without the Unit of Work.
    /// Prefer IUnitOfWork.SaveChangesAsync for multi-operation scenarios.
    /// </summary>
    Task SaveChangesAsync();
}
