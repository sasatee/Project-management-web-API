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
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null);

    }
}
