using System.ComponentModel.DataAnnotations;

namespace WebApplication5.DTOs;

public class CartItemDTO : IIdentifiable
{
    public int CartItemId { get; set; }
    public int? CartId { get; set; }
    public int? Quantity { get; set; }
    public ProductShortDTO? Product { get; set; }

    int IIdentifiable.Id => CartItemId;
}

public class CreateCartItemDTO
{
    [Range(1, int.MaxValue)]
    public int CartId { get; set; }

    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}

public class UpdateCartItemDTO
{
    [Range(1, int.MaxValue)]
    public int CartId { get; set; }

    [Range(1, int.MaxValue)]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}

public class PatchCartItemDTO
{
    [Range(1, int.MaxValue)]
    public int? CartId { get; set; }

    [Range(1, int.MaxValue)]
    public int? ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int? Quantity { get; set; }
}
