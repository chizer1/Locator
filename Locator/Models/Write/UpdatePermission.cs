using System.ComponentModel.DataAnnotations;

namespace Locator.Models.Write;

public class UpdatePermission
{
    [Required]
    public int PermissionId { get; set; }

    [Required]
    public string PermissionName { get; set; }

    [Required]
    public string PermissionDescription { get; set; }
}
