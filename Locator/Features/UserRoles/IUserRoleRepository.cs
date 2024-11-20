namespace Locator.Features.UserRoles;

public interface IUserRoleRepository
{
    public Task<int> AddUserRole(int userId, int roleId);
    public Task DeleteUserRole(int userId, int roleId);
}
