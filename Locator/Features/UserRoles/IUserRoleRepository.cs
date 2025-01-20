namespace Locator.Features.UserRoles;

internal interface IUserRoleRepository
{
    public Task<int> AddUserRole(int userId, int roleId);
    public Task DeleteUserRole(int userId, int roleId);
}
