using Locator.Models.Read;
using Locator.Models.Write;
using Locator.Repositories;

namespace Locator.Services;

internal class RoleService(
    RoleRepository roleRepository,
    UserService userService,
    UserRoleRepository userRoleRepository,
    Auth0Service auth0Service
)
{
    public async Task<List<Role>> GetRoles()
    {
        return await roleRepository.GetRoles();
    }

    public async Task<Role> GetRole(int roleId)
    {
        return await roleRepository.GetRole(roleId);
    }

    public async Task<int> AddRole(AddRole addRole)
    {
        var accessToken = await auth0Service.GetAccessToken();
        var auth0RoleId = await auth0Service.AddRole(
            accessToken,
            addRole.RoleName,
            addRole.RoleDescription
        );

        return await roleRepository.AddRole(auth0RoleId, addRole.RoleName, addRole.RoleDescription);
    }

    // UpdateRole
    // DeleteRole

    public async Task<int> AddUserRole(int userId, int roleId)
    {
        var accessToken = await auth0Service.GetAccessToken();
        var user = await userService.GetUser(userId);
        var role = await GetRole(roleId);

        await auth0Service.AssignUserToRole(accessToken, user.Auth0Id, role.Auth0RoleId);

        return await userRoleRepository.AddUserRole(user.UserId, role.RoleId);
    }

    public async Task DeleteUserRole(int userId, int roleId)
    {
        var accessToken = await auth0Service.GetAccessToken();
        var user = await userService.GetUser(userId);
        var role = await GetRole(roleId);

        await auth0Service.RemoveUserFromRole(accessToken, user.Auth0Id, role.Auth0RoleId);

        await userRoleRepository.DeleteUserRole(user.UserId, role.RoleId);
    }
}
