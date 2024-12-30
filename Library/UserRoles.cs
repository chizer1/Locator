using Locator.Common;
using Locator.Features.Roles;
using Locator.Features.UserRoles;
using Locator.Features.UserRoles.AddUserRole;
using Locator.Features.UserRoles.DeleteUserRole;
using Locator.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace Locator.Library;

internal class UserRoles
{
    private readonly AddUserRole _addUserRole;
    private readonly DeleteUserRole _deleteUserRole;

    public UserRoles(DbContext locatorDb, Auth0 auth0)
    {
        IUserRoleRepository userRoleRepository = new UserRoleRepository(locatorDb);
        IUserRepository userRepository = new UserRepository(locatorDb);
        IRoleRepository roleRepository = new RoleRepository(locatorDb);
        IAuth0UserRoleService auth0UserService = new Auth0UserRoleService(auth0);

        _addUserRole = new AddUserRole(
            userRoleRepository,
            roleRepository,
            userRepository,
            auth0UserService
        );
        _deleteUserRole = new DeleteUserRole(
            userRoleRepository,
            roleRepository,
            userRepository,
            auth0UserService
        );
    }

    public async Task<int> AddUserRole(int userId, int roleId)
    {
        return await _addUserRole.Handle(new AddUserRoleCommand(userId, roleId));
    }

    public async Task DeleteUserRole(int userId, int roleId)
    {
        await _deleteUserRole.Handle(new DeleteUserRoleCommand(userId, roleId));
    }
}
