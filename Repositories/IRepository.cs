namespace WebApplication5.Repositories;

public interface IRepository<T> where T : class
{
    Task<(List<T> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);

    Task<T?> GetByIdAsync(int id);

    Task<T?> FindAsync(int id);

    Task AddAsync(T entity);

    void Remove(T entity);

    int GetId(T entity);

    Task SaveChangesAsync();
}
