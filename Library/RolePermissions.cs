using Locator.Common;
using Locator.Features.RolePermissions;
using Locator.Features.RolePermissions.AddRolePermission;
using Locator.Features.RolePermissions.DeleteRolePermission;
using Locator.Features.Roles;
using Microsoft.EntityFrameworkCore;

namespace Locator.Library;

internal class RolePermissions
{
    private readonly AddRolePermission _addRolePermission;
    private readonly DeleteRolePermission _deleteRolePermission;

    public RolePermissions(DbContext locatorDb, Auth0 auth0)
    {
        IRolePermissionRepository rolePermissionRepository = new RolePermissionRepository(
            locatorDb
        );
        IRoleRepository roleRepository = new RoleRepository(locatorDb);
        IAuth0RolePermissionService auth0RolePermissionService = new Auth0RolePermissionService(
            auth0
        );

        _addRolePermission = new AddRolePermission(
            rolePermissionRepository,
            roleRepository,
            auth0RolePermissionService
        );
        _deleteRolePermission = new DeleteRolePermission(
            rolePermissionRepository,
            roleRepository,
            auth0RolePermissionService
        );
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
