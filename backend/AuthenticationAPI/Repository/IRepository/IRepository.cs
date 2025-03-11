namespace AuthenticationAPI.Repository.IRepository
{
    public interface IRepository<T>  where T:class
    {
        Task<List<T>> GetAll();
        Task<T> FindByIdAsync(Guid id);
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(Guid id);

        Task<int> SaveChangesAsync();


    }
}
