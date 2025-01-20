using Locator.Domain;

namespace Locator.Features.Permissions;

internal interface IPermissionRepository
{
    public Task<int> AddPermission(string permissionName, string permissionDescription);

    public Task<Permission> GetPermission(int permissionId);

    public Task<List<Permission>> GetPermissions();

    public Task UpdatePermission(
        int permissionId,
        string permissionName,
        string permissionDescription
    );

    public Task DeletePermission(int permissionId);
}
