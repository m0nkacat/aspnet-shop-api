using System.ComponentModel.DataAnnotations;
using WebApplication5.Auth;

namespace WebApplication5.DTOs;

public class ReservationDTO : IIdentifiable, IOwnedResource
{
    public int ReservationId { get; set; }
    public int Quantity { get; set; }
    public DateTime ReservedAt { get; set; }
    public DateTime? CanceledAt { get; set; }
    public bool IsActive { get; set; }
    public ProductShortDTO Product { get; set; } = null!;
    public UserShortDTO User { get; set; } = null!;

    int IIdentifiable.Id => ReservationId;

    int IOwnedResource.OwnerId => User.UserId;
}

public class CreateReservationDTO
{
    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Количество должно быть больше 0.")]
    public int Quantity { get; set; }
}
