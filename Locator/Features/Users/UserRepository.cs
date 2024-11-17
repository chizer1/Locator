using System.Data.SqlClient;
using Dapper;
using Locator.Domain;
using Locator.Common.Models;

namespace Locator.Features.Users;

internal class UserRepository(SqlConnection locatorDb) : IUserRepository
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

    public async Task<User> GetUser(string emailAddress)
    {
        return await locatorDb.QuerySingleOrDefaultAsync<User>(
            @$"
            select
                u.Auth0ID {nameof(User.Auth0Id)},
                u.UserID {nameof(User.UserId)},
                u.FirstName {nameof(User.FirstName)},
                u.LastName {nameof(User.LastName)},
                u.EmailAddress {nameof(User.EmailAddress)},
                u.UserStatusID {nameof(User.UserStatus)},
                u.Auth0ID {nameof(User.Auth0Id)}
            from dbo.[User] u
            where
                u.EmailAddress = @EmailAddress",
            new { emailAddress }
        );
    }

    public async Task<User> GetUser(int userId)
    {
        return await locatorDb.QuerySingleOrDefaultAsync<User>(
            @$"
            select
                u.Auth0ID {nameof(User.Auth0Id)},
                u.UserID {nameof(User.UserId)},
                u.FirstName {nameof(User.FirstName)},
                u.LastName {nameof(User.LastName)},
                u.EmailAddress {nameof(User.EmailAddress)},
                u.UserStatusID {nameof(User.UserStatus)}
            from dbo.[User] u
            where
                u.UserId = @UserId",
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
                UserStatusID = (int)userStatus,
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
