using System.Data;
using Dapper;
using Locator.Models;

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
        var userId = await locatorDb.QuerySingleAsync<int>(
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

            select scope_identity() as UserId",
            new
            {
                firstName,
                lastName,
                emailAddress,
                UserStatusID = (int)userStatus,
                auth0Id,
            }
        );

        return userId;
    }

    public async Task<User> GetUser(string auth0Id)
    {
        // get roles for user, use spliton to map to user object dapper User, Role, Connections
        var results = await locatorDb.QueryAsync<User, Role, Connection, User>(
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
                r.Description {nameof(Role.Description)},
                c.ConnectionID {nameof(Connection.ConnectionId)},
                c.DatabaseID {nameof(Connection.Database.DatabaseId)}
            from dbo.[User] u
            inner join dbo.UserRole ur
                on ur.UserID = u.UserID
            inner join dbo.Role r
                on r.RoleID = ur.RoleID
            inner join dbo.ClientUser cu
                on cu.UserID = u.UserID
            inner join dbo.Connection c
                on c.ClientUserID = cu.ClientUserID
            where
                u.Auth0ID = @Auth0ID",
            (user, role, connection) =>
            {
                // roles and connection
                user.Roles.Add(role);
                user.Connections.Add(connection);
                return user;
            },
            new { auth0Id },
            splitOn: $"{nameof(Role.RoleId)}, {nameof(Connection.ConnectionId)}"
        );

        return results.FirstOrDefault();
    }

    public async Task<User> GetUser(int userId)
    {
        return await locatorDb.QuerySingleAsync<User>(
            @$"
            select 
                u.Auth0ID {nameof(User.Auth0Id)},
                u.UserID {nameof(User.UserId)},
                u.FirstName {nameof(User.FirstName)},
                u.LastName {nameof(User.LastName)},
                u.EmailAddress {nameof(User.EmailAddress)},
                u.UserStatusID {nameof(User.UserStatus)}
            from dbo.[User] u
            inner join dbo.UserStatus us
                on us.UserStatusID = u.UserStatusID
            inner join dbo.Client c
                on c.ClientID = u.ClientID
            where
                u.UserID = @UserID",
            new { userId }
        );
    }

    public async Task<List<User>> GetUsers()
    {
        return (
            await locatorDb.QueryAsync<User>(
                @$"
            select
                u.UserID {nameof(User.UserId)},
                u.FirstName {nameof(User.FirstName)},
                u.LastName {nameof(User.LastName)},
                u.EmailAddress {nameof(User.EmailAddress)},
                u.UserStatusID {nameof(User.UserStatus)}
            from dbo.[User] u
            inner join dbo.UserStatus us
                on us.UserStatusID = u.UserStatusID
            inner join dbo.Client c
                on c.ClientID = u.ClientID
            order by
                u.LastName asc"
            )
        ).ToList();
    }

    public async Task<PagedList<User>> GetUsers(
        int clientId,
        string keyword,
        int pageNumber,
        int pageSize
    )
    {
        var whereClause = keyword == null ? "" : " and u.LastName like '%' + @Keyword + '%'";

        var results = await locatorDb.QueryMultipleAsync(
            @$"
            select 
                count(*) as TotalCount 
            from dbo.[User] u
            inner join dbo.UserStatus us
                on us.UserStatusID = u.UserStatusID
            inner join dbo.Client c
                on c.ClientID = u.ClientID
                and c.ClientID = @ClientID
            {whereClause}

            select
                u.UserID {nameof(User.UserId)},
                u.FirstName {nameof(User.FirstName)},
                u.LastName {nameof(User.LastName)},
                u.EmailAddress {nameof(User.EmailAddress)},
                u.UserStatusID {nameof(UserStatus)}
            from dbo.[User] u
            inner join dbo.UserStatus us
                on us.UserStatusID = u.UserStatusID
            inner join dbo.Client c
                on c.ClientID = u.ClientID
                and c.ClientID = @ClientID
            {whereClause}
            order by
                u.LastName asc
            offset ((@PageNumber - 1) * @PageSize) rows
            fetch next @PageSize rows only",
            new
            {
                clientId,
                keyword,
                pageNumber,
                pageSize,
            }
        );

        int rowCount = results.Read<int>().First();
        return new(
            results.Read<User>().ToList(),
            rowCount,
            pageNumber,
            pageSize,
            (int)Math.Ceiling((double)rowCount / pageSize)
        );
    }

    public async Task UpdateUser(
        int userId,
        string firstName,
        string lastName,
        string emailAddress,
        UserStatus userStatus
    )
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
                userId,
                firstName,
                lastName,
                emailAddress,
                userStatusID = (int)userStatus,
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
