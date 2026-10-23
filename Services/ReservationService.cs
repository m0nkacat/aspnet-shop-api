using AutoMapper;
using WebApplication5.DTOs;
using WebApplication5.Models;
using WebApplication5.Repositories;

namespace WebApplication5.Services;

public interface IReservationService
{
    Task<PagedResult<ReservationDTO>> GetPagedAsync(int page, int pageSize);

    Task<ReservationDTO?> GetByIdAsync(int id);

    Task<ServiceResult<ReservationDTO>> ReserveAsync(CreateReservationDTO dto);

    Task<ServiceResult<ReservationDTO>> CancelAsync(int id);
}

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservations;
    private readonly IRepository<Product> _products;
    private readonly IRepository<User> _users;
    private readonly IMapper _mapper;

    public ReservationService(
        IReservationRepository reservations,
        IRepository<Product> products,
        IRepository<User> users,
        IMapper mapper)
    {
        _reservations = reservations;
        _products = products;
        _users = users;
        _mapper = mapper;
    }

    public async Task<PagedResult<ReservationDTO>> GetPagedAsync(int page, int pageSize)
    {
        var (items, totalCount) = await _reservations.GetPagedAsync(page, pageSize);
        return new PagedResult<ReservationDTO>(_mapper.Map<List<ReservationDTO>>(items), totalCount);
    }

    public async Task<ReservationDTO?> GetByIdAsync(int id)
    {
        var reservation = await _reservations.GetByIdAsync(id);
        return reservation is null ? null : _mapper.Map<ReservationDTO>(reservation);
    }

    public async Task<ServiceResult<ReservationDTO>> ReserveAsync(CreateReservationDTO dto)
    {
        var product = await _products.FindAsync(dto.ProductId);
        if (product is null)
        {
            return ServiceResult<ReservationDTO>.Fail(ServiceStatus.NotFound, $"Товар с id={dto.ProductId} не найден.");
        }

        var user = await _users.FindAsync(dto.UserId);
        if (user is null)
        {
            return ServiceResult<ReservationDTO>.Fail(ServiceStatus.NotFound, $"Пользователь с id={dto.UserId} не найден.");
        }

        if (!user.IsActive)
        {
            return ServiceResult<ReservationDTO>.Fail(ServiceStatus.Conflict, $"Пользователь с id={dto.UserId} неактивен.");
        }

        if (product.Stock < dto.Quantity)
        {
            return ServiceResult<ReservationDTO>.Fail(ServiceStatus.Conflict,
                $"Недостаточно товара «{product.ProductName}» на складе: доступно {product.Stock}, запрошено {dto.Quantity}.");
        }

        product.Stock -= dto.Quantity;

        var reservation = _mapper.Map<Reservation>(dto);
        reservation.ReservedAt = DateTime.UtcNow;
        await _reservations.AddAsync(reservation);

        await _reservations.SaveChangesAsync();

        return ServiceResult<ReservationDTO>.Ok(_mapper.Map<ReservationDTO>(reservation));
    }

    public async Task<ServiceResult<ReservationDTO>> CancelAsync(int id)
    {
        var reservation = await _reservations.GetActiveByIdAsync(id);
        if (reservation is null)
        {
            return ServiceResult<ReservationDTO>.Fail(ServiceStatus.NotFound,
                $"Активный резерв с id={id} не найден (не существует или уже отменён).");
        }

        reservation.Product.Stock += reservation.Quantity;

        reservation.CanceledAt = DateTime.UtcNow;

        await _reservations.SaveChangesAsync();

        return ServiceResult<ReservationDTO>.Ok(_mapper.Map<ReservationDTO>(reservation));
    }
}
