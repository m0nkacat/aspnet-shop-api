using System.ComponentModel.DataAnnotations;

namespace WebApplication5.DTOs;

public class OrderItemDTO : IIdentifiable
{
    public int OrderItemId { get; set; }
    public int? OrderId { get; set; }
    public int? Quantity { get; set; }
    public decimal? Price { get; set; }
    public ProductShortDTO? Product { get; set; }

    int IIdentifiable.Id => OrderItemId;
}

public class CreateOrderItemDTO
{
    [Range(1, int.MaxValue)]
    public int OrderId { get; set; }

    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
}

public class UpdateOrderItemDTO
{
    [Range(1, int.MaxValue)]
    public int OrderId { get; set; }

    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
}

public class PatchOrderItemDTO
{
    [Range(1, int.MaxValue)]
    public int? OrderId { get; set; }

    [Range(1, int.MaxValue)]
    public int? ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int? Quantity { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? Price { get; set; }
}
