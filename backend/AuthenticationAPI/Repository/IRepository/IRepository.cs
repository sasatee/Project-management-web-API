using Payroll.Model;
using System.Linq.Expressions;

namespace AuthenticationAPI.Repository.IRepository
{
    public interface IRepository<T>  where T:class
    {
        Task<List<T>> GetAll();
        Task<List<T>> GetAll(Expression<Func<T, bool>> filter);
        Task<T> FindByIdAsync(Guid id);
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(Guid id);
        Task<int> SaveChangesAsync();



        /// <summary>
        /// Retrieves a single entity matching the specified filter expression, optionally including related entities.
        /// </summary>
        /// <param name="filter">A LINQ expression to filter the entity.</param>
        /// <param name="includeProperties">A comma-separated list of related entity property names to include in the query (optional).</param>
        /// <returns>The entity matching the filter, or null if not found.</returns>
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);

    }
}
