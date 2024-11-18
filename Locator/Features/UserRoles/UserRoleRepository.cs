using System.Data;
using Dapper;

namespace Locator.Features.UserRoles;

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

    public async Task DeleteUserRole(int userId, int roleId)
    {
        await locatorDb.ExecuteAsync(
            @$"
            delete from dbo.UserRole
            where UserID = @UserID and RoleID = @RoleID",
            new { userId, roleId }
        );
    }
}
