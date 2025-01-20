namespace Locator.Db;

internal class RoleEntity
{
    public int RoleId { get; set; }

    public string Auth0RoleId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public virtual ICollection<RolePermissionEntity> RolePermissions { get; set; } = [];

    public virtual ICollection<UserRoleEntity> UserRoles { get; set; } = [];
}
