using AuthenticationAPI.Data;
using AuthenticationAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected DbSet<T> dbSet;
        private readonly ApplicationDbContext _dbContext;
        public Repository(ApplicationDbContext appContext)
        {
            dbSet = appContext.Set<T>();
            this._dbContext = appContext;

        }
        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await FindByIdAsync(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        public async Task<T> FindByIdAsync(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetAll()
        {
            var list = await dbSet.ToListAsync();
            return list;
        }

        public async Task<int> SaveChangesAsync()
        {
            
            return await _dbContext.SaveChangesAsync();
            await _dbContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }


    }
}
