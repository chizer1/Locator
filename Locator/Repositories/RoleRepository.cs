using System.Data;
using Dapper;
using Locator.Models;

namespace Locator.Repositories;

internal class RoleRepository(IDbConnection locatorDb)
{
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

    public async Task<int> AddRole(string auth0RoleId, string name, string description)
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
                auth0RoleId,
                name,
                description,
            }
        );
    }

    public async Task UpdateRole(int roleId, string name, string description)
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
                roleId,
                name,
                description,
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

    // add user role
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
}
