namespace Locator.Db;

internal class RolePermissionEntity
{
    public int RolePermissionId { get; set; }

    public int RoleId { get; set; }

    public int PermissionId { get; set; }

    public virtual PermissionEntity Permission { get; set; }

    public virtual RoleEntity Role { get; set; }
}
