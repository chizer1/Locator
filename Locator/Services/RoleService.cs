using Locator.Models;
using Locator.Repositories;

namespace Locator.Services;

internal class RoleService(RoleRepository roleRepository, Auth0Service auth0Service)
{
    public async Task<List<Role>> GetRoles()
    {
        return await roleRepository.GetRoles();
    }

    public async Task<Role> GetRole(int roleId)
    {
        return await roleRepository.GetRole(roleId);
    }

    public async Task<int> AddRole(string name, string description)
    {
        var accessToken = await auth0Service.GetAccessToken();
        var auth0RoleId = await auth0Service.AddRole(accessToken, name, description);

        return await roleRepository.AddRole(auth0RoleId, name, description);
    }

    public async Task<int> AddUserRole(User user, Role role)
    {
        var accessToken = await auth0Service.GetAccessToken();

        await auth0Service.AssignUserToRole(accessToken, user.Auth0Id, role.Auth0RoleId);

        return await roleRepository.AddUserRole(user.UserId, role.RoleId);
    }

    public async Task DeleteUserRole(User user, Role role)
    {
        var accessToken = await auth0Service.GetAccessToken();

        await auth0Service.RemoveUserFromRole(accessToken, user.Auth0Id, role.Auth0RoleId);

        await roleRepository.DeleteUserRole(user.UserId, role.RoleId);
    }
}
