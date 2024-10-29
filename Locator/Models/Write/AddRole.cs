using System.ComponentModel.DataAnnotations;

namespace Locator.Models.Write;

public class AddRole
{
    [Required]
    public string RoleName { get; set; }

    [Required]
    public string RoleDescription { get; set; }
}
