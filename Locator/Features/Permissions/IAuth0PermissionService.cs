using Locator.Domain;

namespace Locator.Features.Permissions;

internal interface IAuth0PermissionService
{
    public Task UpdatePermissions(List<Permission> permissions);
}
