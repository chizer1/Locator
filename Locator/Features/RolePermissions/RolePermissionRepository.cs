using System.Data;
using Dapper;
using Locator.Domain;

namespace Locator.Features.RolePermissions;

internal class RolePermissionRepository(IDbConnection locatorDb) : IRolePermissionRepository
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

    public async Task<List<Permission>> GetRolePermissions(int roleId)
    {
        return (List<Permission>)
            await locatorDb.QueryAsync<Permission>(
                @$"
                select
                    p.PermissionID {nameof(Permission.PermissionId)},
                    p.PermissionName {nameof(Permission.PermissionName)},
                    p.PermissionDescription {nameof(Permission.PermissionDescription)}
                from dbo.Permission p
                inner join dbo.RolePermission r 
                    on p.Permission = r.Permission
                where 
                    r.RoleID = @RoleID",
                new { roleId }
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
