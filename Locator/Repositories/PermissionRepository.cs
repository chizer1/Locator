using System.Data;
using Dapper;
using Locator.Models.Read;
using Locator.Models.Write;

namespace Locator.Repositories;

internal class PermissionRepository(IDbConnection locatorDb)
{
    public async Task<int> AddPermission(string permissionName, string permissionDescription)
    {
        return await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.Permission
            (
                PermissionName,
                PermissionDescription
            )
            values
            (
                @PermissionName,
                @PermissionDescription
            )
            
            select scope_identity()",
            new { permissionName, permissionDescription }
        );
    }

    public async Task<Permission> GetPermission(int permissionId)
    {
        return await locatorDb.QuerySingleAsync<Permission>(
            @$"
            select
                PermissionID {nameof(Permission.PermissionId)},
                PermissionName {nameof(Permission.PermissionName)},
                PermissionDescription {nameof(Permission.PermissionDescription)}
            from dbo.Permission
            where
                PermissionID = @PermissionID",
            new { permissionId }
        );
    }

    public async Task<List<Permission>> GetPermissions()
    {
        return (List<Permission>)
            await locatorDb.QueryAsync<Permission>(
                @$"
                select
                    PermissionID {nameof(Permission.PermissionId)},
                    PermissionName {nameof(Permission.PermissionName)},
                    PermissionDescription {nameof(Permission.PermissionDescription)}
                from dbo.Permission"
            );
    }

    public async Task UpdateRole(UpdatePermission updatePermission)
    {
        await locatorDb.ExecuteAsync(
            @$"
            update dbo.Permission
            set
                PermissionName = @PermissionName,
                PermissionDescription = @PermissionDescription
            where
                Permission = @PermissionID",
            new
            {
                updatePermission.PermissionId,
                updatePermission.PermissionName,
                updatePermission.PermissionDescription,
            }
        );
    }

    public async Task DeletePermission(int permissionId)
    {
        await locatorDb.ExecuteAsync(
            @$"
            delete from dbo.Permission
            where
                PermissionID = @PermissionID",
            new { permissionId }
        );
    }
}
