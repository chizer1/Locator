using System.Data.SqlClient;
using Locator.Features.RolePermissions;
using Locator.Features.RolePermissions.AddRolePermission;
using Locator.Features.RolePermissions.DeleteRolePermission;

namespace Locator.Library;

public class RolePermissions
{
    private readonly AddRolePermission _addRolePermission;
    private readonly DeleteRolePermission _deleteRolePermission;

    public RolePermissions(SqlConnection locatorDb)
    {
        IRolePermissionRepository rolePermission = new RolePermissionRepository(locatorDb);

        _addRolePermission = new AddRolePermission(rolePermission);
        _deleteRolePermission = new DeleteRolePermission(rolePermission);
    }

    public async Task<int> AddRolePermission(int roleId, int permissionId)
    {
        return await _addRolePermission.Handle(new AddRolePermissionCommand(roleId, permissionId));
    }

    public async Task DeleteRolePermission(int roleId, int permissionId)
    {
        await _deleteRolePermission.Handle(new DeleteRolePermissionCommand(roleId, permissionId));
    }
}
