namespace OnlineStore.Repository.Interfaces
{
    public interface IRepo<T> where T : class
    {
        Task<IEnumerable<T>?> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveAsync();
        Task<bool> IsExistAsync(int id);
    }
}
