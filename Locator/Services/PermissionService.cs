using Locator.Models.Read;
using Locator.Models.Write;
using Locator.Repositories;

namespace Locator.Services;

internal class PermissionService(
    PermissionRepository permissionRepository,
    Auth0Service auth0Service
)
{
    public async Task<int> AddPermission(string permissionName, string permissionDescription)
    {
        var accessToken = await auth0Service.GetAccessToken();

        //await auth0Service.UpdatePermission(accessToken, permissionName, permissionDescription);

        return await permissionRepository.AddPermission(permissionName, permissionDescription);
    }
}
