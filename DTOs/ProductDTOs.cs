using System.ComponentModel.DataAnnotations;

namespace WebApplication5.DTOs;

public class ProductDTO : IIdentifiable
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal? Price { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public int Stock { get; set; }
    public DateTime CreatedAt { get; set; }
    public CategoryShortDTO? Category { get; set; }

    int IIdentifiable.Id => ProductId;
}

public class ProductShortDTO
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public decimal? Price { get; set; }
    public int Stock { get; set; }
}

public class CreateProductDTO
{
    [Required, MaxLength(100)]
    public string ProductName { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Image { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}

public class UpdateProductDTO
{
    [Required, MaxLength(100)]
    public string ProductName { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Image { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}

public class PatchProductDTO
{
    [MaxLength(100)]
    public string? ProductName { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? Price { get; set; }

    [Range(1, int.MaxValue)]
    public int? CategoryId { get; set; }

    public string? Description { get; set; }

    [MaxLength(200)]
    public string? Image { get; set; }

    [Range(0, int.MaxValue)]
    public int? Stock { get; set; }
}
