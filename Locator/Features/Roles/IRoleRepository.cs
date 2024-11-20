using Locator.Domain;

namespace Locator.Features.Roles;

public interface IRoleRepository
{
    public Task<int> AddRole(string auth0RoleId, string roleName, string roleDescription);
    public Task<Role> GetRole(int roleId);
    public Task<List<Role>> GetRoles();
    public Task UpdateRole(int roleId, string roleName, string roleDescription);
    public Task DeleteRole(int roleId);
}
