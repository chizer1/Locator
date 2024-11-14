using System.ComponentModel.DataAnnotations;

namespace Locator.Models.Write;

public abstract class AddRole(string roleName, string roleDescription)
{
    [Required]
    public string RoleName { get; } = roleName;

    [Required]
    public string RoleDescription { get; } = roleDescription;
}
