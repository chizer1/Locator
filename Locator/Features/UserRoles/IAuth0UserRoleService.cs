namespace Locator.Features.UserRoles;

public interface IAuth0UserRoleService
{
    public Task AddUserRole(string auth0Id, string auth0RoleId);
    public Task DeleteUserRole(string auth0Id, string auth0RoleId);
}
