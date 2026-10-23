using System.ComponentModel.DataAnnotations;

namespace WebApplication5.DTOs;

public class CartDTO : IIdentifiable
{
    public int CartId { get; set; }
    public UserShortDTO? User { get; set; }
    public List<CartItemDTO> CartItems { get; set; } = new();

    int IIdentifiable.Id => CartId;
}

public class CreateCartDTO
{
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }
}

public class UpdateCartDTO
{
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }
}

public class PatchCartDTO
{
    [Range(1, int.MaxValue)]
    public int? UserId { get; set; }
}
