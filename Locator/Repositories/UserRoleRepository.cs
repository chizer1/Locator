using System.Data;
using Dapper;

namespace Locator.Repositories;

internal class UserRoleRepository(IDbConnection locatorDb)
{
    public async Task<int> AddUserRole(int userId, int roleId)
    {
        return await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.UserRole
            (
                UserID,
                RoleID
            )
            values
            (
                @UserID,
                @RoleID
            )

            select scope_identity()",
            new { userId, roleId }
        );
    }

    public async Task DeleteUserRole(int userRoleId)
    {
        await locatorDb.ExecuteAsync(
            @$"
            delete from dbo.UserRole
            where UserRoleID = @UserRoleID",
            new { userRoleId }
        );
    }
}
