using System.Data;
using Dapper;

namespace Locator.Features.RolePermissions;

internal class RolePermissionRepository(IDbConnection locatorDb)
{
    public async Task<int> AddRolePermission(int roleId, int permissionId)
    {
        return await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.RolePermission
            (
                RoleID,
                PermissionID
            )
            values
            (
                @RoleID,
                @PermissionID
            )

            select scope_identity()",
            new { roleId, permissionId }
        );
    }

    public async Task DeleteRolePermission(int roleId, int permissionId)
    {
        await locatorDb.ExecuteAsync(
            @$"
            delete from dbo.RolePermission
            where PermissionID = @PermissionID and RoleID = @RoleID",
            new { roleId, permissionId }
        );
    }
}
