namespace Locator.Db;

internal class PermissionEntity
{
    public int PermissionId { get; set; }

    public string PermissionName { get; set; }

    public string PermissionDescription { get; set; }

    public virtual ICollection<RolePermissionEntity> RolePermissions { get; set; } = [];
}
