using Locator.Domain;

namespace Locator.Features.RolePermissions;

internal interface IAuth0RolePermissionService
{
    public Task UpdateRolePermissions(string auth0RoleId, List<Permission> permissions);
}
