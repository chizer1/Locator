using Locator.Domain;

namespace Locator.Features.RolePermissions;

public interface IRolePermissionRepository
{
    public Task<int> AddRolePermission(int roleId, int permissionId);
    public Task<List<Permission>> GetRolePermissions(int roleId);
    public Task DeleteRolePermission(int roleId, int permissionId);
}
