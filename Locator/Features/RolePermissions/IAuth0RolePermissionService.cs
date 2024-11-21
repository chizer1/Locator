using Locator.Domain;

namespace Locator.Features.RolePermissions;

public interface IAuth0RolePermissionService
{
    public Task UpdateRolePermissions(string auth0RoleId, List<Permission> permissions);
}
