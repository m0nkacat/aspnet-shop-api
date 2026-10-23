using System.ComponentModel.DataAnnotations;

namespace WebApplication5.DTOs;

public class RoleDTO : IIdentifiable
{
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public List<UserShortDTO> Users { get; set; } = new();

    int IIdentifiable.Id => RoleId;
}

public class RoleShortDTO
{
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
}

public class CreateRoleDTO
{
    [Required, MaxLength(50)]
    public string RoleName { get; set; } = string.Empty;
}

public class UpdateRoleDTO
{
    [Required, MaxLength(50)]
    public string RoleName { get; set; } = string.Empty;
}

public class PatchRoleDTO
{
    [MaxLength(50)]
    public string? RoleName { get; set; }
}
