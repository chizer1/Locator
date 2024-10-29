using System.Data;
using Dapper;
using Locator.Models.Read;
using Locator.Models.Write;

namespace Locator.Repositories;

internal class RoleRepository(IDbConnection locatorDb)
{
    public async Task<int> AddRole(string auth0RoleId, string roleName, string roleDescription)
    {
        return await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.Role
            (
                Auth0RoleID,
                Name,
                Description
            )
            values
            (
                @Auth0RoleID,
                @Name,
                @Description
            )
            
            select scope_identity()",
            new
            {
                Auth0RoleID = auth0RoleId,
                Name = roleName,
                Description = roleDescription,
            }
        );
    }

    public async Task<Role> GetRole(int roleId)
    {
        return await locatorDb.QuerySingleAsync<Role>(
            @$"
            select
                RoleID {nameof(Role.RoleId)},
                Auth0RoleID {nameof(Role.Auth0RoleId)},
                Name {nameof(Role.Name)},
                Description {nameof(Role.Description)}
            from dbo.Role
            where
                RoleID = @RoleID",
            new { roleId }
        );
    }

    public async Task<List<Role>> GetRoles()
    {
        return (List<Role>)
            await locatorDb.QueryAsync<Role>(
                @$"
                select
                    RoleID {nameof(Role.RoleId)},
                    Auth0RoleID {nameof(Role.Auth0RoleId)},
                    Name {nameof(Role.Name)},
                    Description {nameof(Role.Description)}
                from dbo.Role"
            );
    }

    public async Task UpdateRole(UpdateRole updateRole)
    {
        await locatorDb.ExecuteAsync(
            @$"
            update dbo.Role
            set
                Name = @Name,
                Description = @Description
            where
                RoleID = @RoleID",
            new
            {
                updateRole.RoleId,
                updateRole.RoleName,
                updateRole.RoleDescription,
            }
        );
    }

    public async Task DeleteRole(int roleId)
    {
        await locatorDb.ExecuteAsync(
            @$"
            delete from dbo.Role
            where
                RoleID = @RoleID",
            new { roleId }
        );
    }
}
