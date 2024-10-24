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
        int[] roleIds,
        int userStatusId,
        int clientId,
        string auth0Id,
        int createById
    )
    {
        var userId = await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.[User]
            (
                ClientID,
                FirstName,
                LastName,
                EmailAddress,
                UserStatusID,
                Auth0ID,
                CreateByID,
                ModifyByID
            )
            values
            (
                @ClientID,
                @FirstName,
                @LastName,
                @EmailAddress,
                @UserStatusID,
                @Auth0ID,
                @CreateByID,
                @CreateByID
            )

            select scope_identity() as UserId",
            new
            {
                clientId,
                firstName,
                lastName,
                emailAddress,
                userStatusId,
                auth0Id,
                createById,
            }
        );

        foreach (var roleId in roleIds)
        {
            await locatorDb.ExecuteAsync(
                @$"
                insert into dbo.UserRole
                (
                    UserID,
                    RoleID,
                    CreateByID,
                    ModifyByID
                )
                values
                (
                    @UserID,
                    @RoleID,
                    @CreateByID,
                    @CreateByID
                )",
                new
                {
                    userId,
                    roleId,
                    createById,
                }
            );
        }

        return userId;
    }

    public async Task<User> GetUser(string auth0Id)
    {
        return await locatorDb.QuerySingleAsync<User>(
            @$"
            select 
                u.Auth0ID {nameof(User.Auth0Id)},
                u.UserID {nameof(User.UserId)},
                u.FirstName {nameof(User.FirstName)},
                u.LastName {nameof(User.LastName)},
                u.EmailAddress {nameof(User.EmailAddress)},
                u.UserStatusID {nameof(User.UserStatusId)},
                c.ClientID {nameof(User.ClientId)}
            from dbo.[User] u
            inner join dbo.UserStatus us
                on us.UserStatusID = u.UserStatusID
            inner join dbo.Client c
                on c.ClientID = u.ClientID
            where
                u.Auth0ID = @Auth0ID",
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
                u.UserStatusID {nameof(User.UserStatusId)},
                c.ClientID {nameof(User.ClientId)}
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
                u.UserStatusID {nameof(User.UserStatusId)}
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
                u.UserStatusID {nameof(User.UserStatusId)}
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
        int userStatusId,
        int clientId,
        int[] roleIds,
        int modifyById
    )
    {
        await locatorDb.ExecuteAsync(
            @$"
            update [dbo].[User]
            set
                FirstName = @FirstName,
                LastName = @LastName,
                EmailAddress = @EmailAddress,
                UserStatusID = @UserStatusID,
                ClientID = @ClientID,
                ModifyByID = @ModifyByID,
                ModifyDate = getutcdate()
            where
                UserID = @UserID",
            new
            {
                userId,
                firstName,
                lastName,
                emailAddress,
                userStatusId,
                clientId,
                modifyById,
            }
        );

        await locatorDb.ExecuteAsync(
            @$"
            delete from dbo.UserRole
            where
                UserID = @UserID",
            new { userId }
        );

        foreach (var roleId in roleIds)
        {
            await locatorDb.ExecuteAsync(
                @$"
                insert into dbo.UserRole
                (
                    UserID,
                    RoleID,
                    CreateByID,
                    ModifyByID
                )
                values
                (
                    @UserID,
                    @RoleID,
                    @ModifyByID,
                    @ModifyByID
                )",
                new
                {
                    userId,
                    roleId,
                    modifyById,
                }
            );
        }
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
