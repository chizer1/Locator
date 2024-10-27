using Locator.Models;
using Locator.Repositories;

namespace Locator.Services;

internal class RoleService(RoleRepository roleRepository)
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
        var auth0RoleId = "123";

        return await roleRepository.AddRole(auth0RoleId, name, description);
    }
}
