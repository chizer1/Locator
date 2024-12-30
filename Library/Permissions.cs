using Locator.Common;
using Locator.Domain;
using Locator.Features.Permissions;
using Locator.Features.Permissions.AddPermission;
using Locator.Features.Permissions.DeletePermission;
using Locator.Features.Permissions.GetPermissions;
using Locator.Features.Permissions.UpdatePermission;
using Microsoft.EntityFrameworkCore;

namespace Locator.Library;

internal class Permissions
{
    private readonly AddPermission _addPermission;
    private readonly GetPermissions _getPermissions;
    private readonly DeletePermission _deletePermission;
    private readonly UpdatePermission _updatePermission;

    public Permissions(DbContext locatorDb, Auth0 auth0)
    {
        IPermissionRepository permissionTypeRepository = new PermissionRepository(locatorDb);
        IAuth0PermissionService auth0PermissionService = new Auth0PermissionService(auth0);

        _addPermission = new AddPermission(permissionTypeRepository, auth0PermissionService);
        _getPermissions = new GetPermissions(permissionTypeRepository);
        _deletePermission = new DeletePermission(permissionTypeRepository, auth0PermissionService);
        _updatePermission = new UpdatePermission(permissionTypeRepository, auth0PermissionService);
    }

    public async Task<int> AddPermission(string permissionName, string permissionDescription)
    {
        return await _addPermission.Handle(
            new AddPermissionCommand(permissionName, permissionDescription)
        );
    }

    public async Task<List<Permission>> GetPermissions()
    {
        return await _getPermissions.Handle(new GetPermissionsQuery());
    }

    public async Task UpdatePermission(
        int permissionId,
        string permissionName,
        string permissionDescription
    )
    {
        await _updatePermission.Handle(
            new UpdatePermissionCommand(permissionId, permissionName, permissionDescription)
        );
    }

    public async Task DeletePermission(int permissionId)
    {
        await _deletePermission.Handle(new DeletePermissionCommand(permissionId));
    }
}
