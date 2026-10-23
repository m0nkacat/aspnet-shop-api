using Microsoft.EntityFrameworkCore;
using WebApplication5.Models;

namespace WebApplication5.Repositories;

public interface IReservationRepository : IRepository<Reservation>
{
    Task<Reservation?> GetActiveByIdAsync(int id);
}

public class ReservationRepository(ShopDbContext context) : Repository<Reservation>(context), IReservationRepository
{
    protected override IQueryable<Reservation> WithDetails(IQueryable<Reservation> query) =>
        query.Include(r => r.Product).Include(r => r.User);

    public Task<Reservation?> GetActiveByIdAsync(int id) =>
        WithDetails(Set).FirstOrDefaultAsync(r => r.ReservationId == id && r.CanceledAt == null);
}
