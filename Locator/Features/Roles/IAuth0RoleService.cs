namespace Locator.Features.Roles;

public interface IAuth0RoleService
{
    public Task<string> AddRole(string roleName, string roleDescription);
    public Task DeleteRole(string auth0RoleId);
    public Task UpdateRole(string auth0RoleId, string roleName, string roleDescription);
}
