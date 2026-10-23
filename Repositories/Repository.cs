using Microsoft.EntityFrameworkCore;
using WebApplication5.Models;

namespace WebApplication5.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ShopDbContext Context;
    protected readonly DbSet<T> Set;
    private readonly string _keyName;

    public Repository(ShopDbContext context)
    {
        Context = context;
        Set = context.Set<T>();
        _keyName = context.Model.FindEntityType(typeof(T))!.FindPrimaryKey()!.Properties[0].Name;
    }

    protected virtual IQueryable<T> WithDetails(IQueryable<T> query) => query;

    public async Task<(List<T> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var totalCount = await Set.CountAsync();

        var items = await WithDetails(Set.AsNoTracking())
            .OrderBy(e => EF.Property<int>(e, _keyName))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public Task<T?> GetByIdAsync(int id) =>
        WithDetails(Set).FirstOrDefaultAsync(e => EF.Property<int>(e, _keyName) == id);

    public async Task<T?> FindAsync(int id) => await Set.FindAsync(id);

    public async Task AddAsync(T entity) => await Set.AddAsync(entity);

    public void Remove(T entity) => Set.Remove(entity);

    public int GetId(T entity) => (int)Context.Entry(entity).Property(_keyName).CurrentValue!;

    public Task SaveChangesAsync() => Context.SaveChangesAsync();
}
