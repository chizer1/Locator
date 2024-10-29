using System.ComponentModel.DataAnnotations;

namespace Locator.Models.Write;

public class UpdateRole
{
    [Required]
    public int RoleId { get; set; }

    [Required]
    public string RoleName { get; set; }

    [Required]
    public string RoleDescription { get; set; }
}
