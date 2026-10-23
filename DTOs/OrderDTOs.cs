using System.ComponentModel.DataAnnotations;

namespace WebApplication5.DTOs;

public class OrderDTO : IIdentifiable
{
    public int OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public UserShortDTO? User { get; set; }
    public List<OrderItemDTO> OrderItems { get; set; } = new();

    int IIdentifiable.Id => OrderId;
}

public class CreateOrderDTO
{
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    public DateTime? OrderDate { get; set; }
}

public class UpdateOrderDTO
{
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    [Required]
    public DateTime? OrderDate { get; set; }
}

public class PatchOrderDTO
{
    [Range(1, int.MaxValue)]
    public int? UserId { get; set; }

    public DateTime? OrderDate { get; set; }
}
