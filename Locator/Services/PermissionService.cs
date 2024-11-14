using Locator.Models.Read;
using Locator.Models.Write;
using Locator.Repositories;

namespace Locator.Services;

internal class PermissionService(
    PermissionRepository permissionRepository,
    RolePermissionRepository rolePermissionRepository,
    RoleRepository roleRepository,
    Auth0Service auth0Service
)
{
    public async Task<int> AddPermission(string permissionName, string permissionDescription)
    {
        var accessToken = await auth0Service.GetAccessToken();

        var permissions = await permissionRepository.GetPermissions();
        permissions.Add(
            new Permission
            {
                PermissionName = permissionName,
                PermissionDescription = permissionDescription,
            }
        );

        await auth0Service.UpdatePermissions(accessToken, permissions);

        return await permissionRepository.AddPermission(permissionName, permissionDescription);
    }

    public async Task UpdatePermission(
        int permissionId,
        string permissionName,
        string permissionDescription
    )
    {
        var accessToken = await auth0Service.GetAccessToken();

        // auth0 stuff here

        await permissionRepository.UpdatePermission(
            permissionId,
            permissionName,
            permissionDescription
        );
    }

    public async Task<int> AddRolePermission(int roleId, int permissionId)
    {
        var accessToken = await auth0Service.GetAccessToken();

        var permission = await permissionRepository.GetPermission(permissionId);
        var role = await roleRepository.GetRole(roleId);

        await auth0Service.AddPermissionToRole(
            accessToken,
            permission.PermissionName,
            role.Auth0RoleId
        );

        return await rolePermissionRepository.AddRolePermission(roleId, permissionId);
    }

    public async Task DeletePermission(int permissionId)
    {
        var accessToken = await auth0Service.GetAccessToken();

        var permission = await permissionRepository.GetPermission(permissionId);

        // auth0 stuff in here

        await permissionRepository.DeletePermission(permissionId);
    }

    public async Task DeleteRolePermission(int roleId, int permissionId)
    {
        var accessToken = await auth0Service.GetAccessToken();

        var permission = await permissionRepository.GetPermission(permissionId);
        var role = await roleRepository.GetRole(roleId);

        // auth0 stuff in here

        await rolePermissionRepository.DeleteRolePermission(roleId, permissionId);
    }
}
