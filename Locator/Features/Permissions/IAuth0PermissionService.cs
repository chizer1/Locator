using Locator.Domain;

namespace Locator.Features.Permissions;

public interface IAuth0PermissionService
{
    public Task UpdatePermissions(List<Permission> permissions);
}
