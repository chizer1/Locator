namespace Locator.Features.RolePermissions;

public interface IRolePermissionRepository
{
    public Task<int> AddRolePermission(int roleId, int permissionId);
    public Task DeleteRolePermission(int roleId, int permissionId);
}
