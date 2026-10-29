using Microsoft.EntityFrameworkCore;
using WebApplication5.Models;

namespace WebApplication5.Repositories;

public interface IReservationRepository : IRepository<Reservation>
{
    Task<Reservation?> GetActiveByIdAsync(int id);

    Task<(List<Reservation> Items, int TotalCount)> GetPagedByUserAsync(int userId, int page, int pageSize);
}

public class ReservationRepository(ShopDbContext context) : Repository<Reservation>(context), IReservationRepository
{
    protected override IQueryable<Reservation> WithDetails(IQueryable<Reservation> query) =>
        query.Include(r => r.Product).Include(r => r.User);

    public Task<Reservation?> GetActiveByIdAsync(int id) =>
        WithDetails(Set).FirstOrDefaultAsync(r => r.ReservationId == id && r.CanceledAt == null);

    public async Task<(List<Reservation> Items, int TotalCount)> GetPagedByUserAsync(int userId, int page, int pageSize)
    {
        var query = Set.Where(r => r.UserId == userId);
        var totalCount = await query.CountAsync();

        var items = await WithDetails(query.AsNoTracking())
            .OrderByDescending(r => r.ReservationId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
