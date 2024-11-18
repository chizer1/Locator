using System.Data;
using Dapper;
using Locator.Domain;

namespace Locator.Features.Roles;

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
                Name {nameof(Role.RoleName)},
                Description {nameof(Role.RoleDescription)}
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
                    Name {nameof(Role.RoleName)},
                    Description {nameof(Role.RoleDescription)}
                from dbo.Role"
            );
    }

    public async Task UpdateRole(int roleId, string roleName, string roleDescription)
    {
        await locatorDb.ExecuteAsync(
            @$"
            update dbo.Role
            set
                Name = @RoleName,
                Description = @RoleDescription
            where
                RoleID = @RoleID",
            new
            {
                roleId,
                roleName,
                roleDescription,
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
