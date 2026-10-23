using System.ComponentModel.DataAnnotations;

namespace WebApplication5.DTOs;

public class UserDTO : IIdentifiable
{
    public int UserId { get; set; }
    public string? Username { get; set; }
    public bool IsActive { get; set; }
    public RoleShortDTO? Role { get; set; }

    int IIdentifiable.Id => UserId;
}

public class UserShortDTO
{
    public int UserId { get; set; }
    public string? Username { get; set; }
}

public class CreateUserDTO
{
    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, MinLength(4)]
    public string Password { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int RoleId { get; set; }

    public bool IsActive { get; set; } = true;
}

public class UpdateUserDTO
{
    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required, MinLength(4)]
    public string Password { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int RoleId { get; set; }

    public bool IsActive { get; set; }
}

public class PatchUserDTO
{
    [MaxLength(50)]
    public string? Username { get; set; }

    [MinLength(4)]
    public string? Password { get; set; }

    [Range(1, int.MaxValue)]
    public int? RoleId { get; set; }

    public bool? IsActive { get; set; }
}
