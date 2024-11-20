using System.Data.SqlClient;
using Locator.Features.UserRoles;
using Locator.Features.UserRoles.AddUserRole;
using Locator.Features.UserRoles.DeleteUserRole;

namespace Locator.Library;

public class UserRoles
{
    private readonly AddUserRole _addUserRole;
    private readonly DeleteUserRole _deleteUserRole;

    public UserRoles(SqlConnection locatorDb)
    {
        IUserRoleRepository clientRepository = new UserRoleRepository(locatorDb);

        _addUserRole = new AddUserRole(clientRepository);
        _deleteUserRole = new DeleteUserRole(clientRepository);
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
