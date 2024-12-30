using Locator.Domain;

namespace Locator.Features.RolePermissions;

internal interface IRolePermissionRepository
{
    public Task<int> AddRolePermission(int roleId, int permissionId);
    public Task<List<Permission>> GetRolePermissions(int roleId);
    public Task DeleteRolePermission(int roleId, int permissionId);
}
