using System.ComponentModel.DataAnnotations;

namespace WebApplication5.DTOs;

public class CategoryDTO : IIdentifiable
{
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public List<ProductShortDTO> Products { get; set; } = new();

    int IIdentifiable.Id => CategoryId;
}

public class CategoryShortDTO
{
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
}

public class CreateCategoryDTO
{
    [Required, MaxLength(100)]
    public string CategoryName { get; set; } = string.Empty;
}

public class UpdateCategoryDTO
{
    [Required, MaxLength(100)]
    public string CategoryName { get; set; } = string.Empty;
}

public class PatchCategoryDTO
{
    [MaxLength(100)]
    public string? CategoryName { get; set; }
}
