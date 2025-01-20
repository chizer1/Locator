namespace Locator.Features.UserRoles;

internal interface IAuth0UserRoleService
{
    public Task AddUserRole(string auth0Id, string auth0RoleId);
    public Task DeleteUserRole(string auth0Id, string auth0RoleId);
}
