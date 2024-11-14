using System.ComponentModel.DataAnnotations;

namespace Locator.Models.Write;

public abstract class UpdateRole(int roleId, string roleName, string roleDescription)
{
    [Required]
    public int RoleId { get; } = roleId;

    [Required]
    public string RoleName { get; } = roleName;

    [Required]
    public string RoleDescription { get; } = roleDescription;
}
