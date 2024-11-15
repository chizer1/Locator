using System.Data;
using Dapper;
using Locator.Domain;
using Locator.Models.Read;
using Locator.Models.Write;

namespace Locator.Repositories;

internal class UserRepository(IDbConnection locatorDb)
{
    public async Task<int> AddUser(
        string firstName,
        string lastName,
        string emailAddress,
        UserStatus userStatus,
        string auth0Id
    )
    {
        return await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.[User]
            (
                FirstName,
                LastName,
                EmailAddress,
                UserStatusID,
                Auth0ID
            )
            values
            (
                @FirstName,
                @LastName,
                @EmailAddress,
                @UserStatusID,
                @Auth0ID
            )

            select scope_identity()",
            new
            {
                firstName,
                lastName,
                emailAddress,
                UserStatusID = (int)userStatus,
                auth0Id,
            }
        );
    }

    public async Task<User> GetUser(string auth0Id)
    {
        var results = await locatorDb.QueryAsync<User, Role, User>(
            @$"
            select
                u.UserID {nameof(User.UserId)},
                u.FirstName {nameof(User.FirstName)},
                u.LastName {nameof(User.LastName)},
                u.EmailAddress {nameof(User.EmailAddress)},
                u.UserStatusID {nameof(User.UserStatus)},
                u.Auth0ID {nameof(User.Auth0Id)},
                r.RoleID {nameof(Role.RoleId)},
                r.Auth0RoleID {nameof(Role.Auth0RoleId)},
                r.Name {nameof(Role.Name)},
                r.Description {nameof(Role.Description)}
            from dbo.[User] u
            left join dbo.UserRole ur
                on ur.UserID = u.UserID
            left join dbo.Role r
                on r.RoleID = ur.RoleID
            where
                u.Auth0ID = @Auth0ID",
            (user, role) =>
            {
                user.Roles ??= [];

                if (role == null)
                    return user;
                if (user.Roles.All(r => r.RoleId != role.RoleId))
                    user.Roles.Add(role);

                return user;
            },
            new { Auth0ID = auth0Id },
            splitOn: $"{nameof(Role.RoleId)}"
        );

        return results.FirstOrDefault();
    }

    public async Task<User> GetUser(int userId)
    {
        var results = await locatorDb.QueryAsync<User, Role, User>(
            @$"
            select
                u.UserID {nameof(User.UserId)},
                u.FirstName {nameof(User.FirstName)},
                u.LastName {nameof(User.LastName)},
                u.EmailAddress {nameof(User.EmailAddress)},
                u.UserStatusID {nameof(User.UserStatus)},
                u.Auth0ID {nameof(User.Auth0Id)},
                r.RoleID {nameof(Role.RoleId)},
                r.Auth0RoleID {nameof(Role.Auth0RoleId)},
                r.Name {nameof(Role.Name)},
                r.Description {nameof(Role.Description)}
            from dbo.[User] u
            left join dbo.UserRole ur
                on ur.UserID = u.UserID
            left join dbo.Role r
                on r.RoleID = ur.RoleID
            where
                u.UserId = @UserId",
            (user, role) =>
            {
                user.Roles ??= [];

                if (role == null)
                    return user;
                if (user.Roles.All(r => r.RoleId != role.RoleId))
                    user.Roles.Add(role);

                return user;
            },
            new { userId },
            splitOn: $"{nameof(Role.RoleId)}"
        );

        return results.FirstOrDefault();
    }

    public async Task<List<User>> GetUsers()
    {
        return (
            await locatorDb.QueryAsync<User>(
                @$"
                select
                    u.UserID {nameof(User.UserId)},
                    u.Auth0ID {nameof(User.Auth0Id)},
                    u.FirstName {nameof(User.FirstName)},
                    u.LastName {nameof(User.LastName)},
                    u.EmailAddress {nameof(User.EmailAddress)},
                    u.UserStatusID {nameof(User.UserStatus)}
                from dbo.[User] u
                inner join dbo.UserStatus us
                    on us.UserStatusID = u.UserStatusID
                order by
                    u.LastName asc"
            )
        ).ToList();
    }

    public async Task<PagedList<User>> GetUsers(string keyword, int pageNumber, int pageSize)
    {
        var whereClause = keyword == null ? "" : " and u.LastName like '%' + @Keyword + '%'";

        var results = await locatorDb.QueryMultipleAsync(
            @$"
            select 
                count(*) as TotalCount 
            from dbo.[User] u
            inner join dbo.UserStatus us
                on us.UserStatusID = u.UserStatusID
            {whereClause}

            select
                u.UserID {nameof(User.UserId)},
                u.Auth0ID {nameof(User.Auth0Id)},
                u.FirstName {nameof(User.FirstName)},
                u.LastName {nameof(User.LastName)},
                u.EmailAddress {nameof(User.EmailAddress)},
                u.UserStatusID {nameof(UserStatus)}
            from dbo.[User] u
            inner join dbo.UserStatus us
                on us.UserStatusID = u.UserStatusID
            {whereClause}
            order by
                u.LastName asc
            offset ((@PageNumber - 1) * @PageSize) rows
            fetch next @PageSize rows only",
            new
            {
                keyword,
                pageNumber,
                pageSize,
            }
        );

        var rowCount = results.Read<int>().First();
        return new PagedList<User>(
            results.Read<User>().ToList(),
            rowCount,
            pageNumber,
            pageSize,
            (int)Math.Ceiling((double)rowCount / pageSize)
        );
    }

    public async Task UpdateUser(UpdateUser updateUser)
    {
        await locatorDb.ExecuteAsync(
            @$"
            update [dbo].[User]
            set
                FirstName = @FirstName,
                LastName = @LastName,
                EmailAddress = @EmailAddress,
                UserStatusID = @UserStatusID
            where
                UserID = @UserID",
            new
            {
                updateUser.UserId,
                updateUser.FirstName,
                updateUser.LastName,
                updateUser.EmailAddress,
                UserStatusID = (int)updateUser.UserStatus,
            }
        );
    }

    public async Task DeleteUser(int userId)
    {
        await locatorDb.ExecuteAsync(
            @"
            delete from dbo.UserRole where UserID = @UserID
            delete from dbo.[User] where UserID = @UserID",
            new { userId }
        );
    }

    public async Task DeleteUser(string auth0Id)
    {
        await locatorDb.ExecuteAsync(
            @"
            delete from dbo.UserRole where UserID = (select UserID from dbo.[User] where Auth0ID = @Auth0ID)
            delete from dbo.[User] where Auth0ID = @Auth0ID",
            new
            {
                Auth0ID = new DbString
                {
                    Value = auth0Id,
                    IsFixedLength = false,
                    IsAnsi = true,
                    Length = 50,
                },
            }
        );
    }
}
