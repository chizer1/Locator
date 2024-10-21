using System.Data;
using Dapper;
using Locator.Models;

namespace Locator.Repositories;

public class LocatorRepository(IDbConnection locatorDb)
{
    public ClientStatus GetClientStatus(string auth0Id)
    {
        return (ClientStatus)
            locatorDb.QuerySingle<int>(
                @$"
                select
                    c.ClientStatusID {nameof(Client.ClientStatusId)}
                from dbo.[User] u
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

    public UserStatus GetUserStatus(string auth0Id)
    {
        return (UserStatus)
            locatorDb.QuerySingle<int>(
                @$"
                select
                    u.UserStatusID {nameof(User.UserStatusId)}
                from dbo.[User] u
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

    public Connection GetConnection(string auth0Id, int databaseTypeId)
    {
        return locatorDb.QuerySingle<Connection>(
            $@"
            select
                c.ClientID {nameof(Connection.ClientId)},
                c.ClientCode {nameof(Connection.ClientCode)},
                u.UserID {nameof(Connection.UserId)},
                ds.[DatabaseServerName] {nameof(Connection.DatabaseServer)},
                d.[DatabaseName] {nameof(Connection.DatabaseName)},
                d.[DatabaseUser] {nameof(Connection.DatabaseUser)},
                d.[DatabaseUserPassword] {nameof(Connection.DatabaseUserPassword)}
            from [User] u
            inner join [Client] c
                on u.ClientID = c.ClientID
            inner join [ClientConnection] cc  
                on cc.ClientID = c.ClientID
            inner join [Database] d
                on d.DatabaseID = cc.DatabaseID
            inner join [DatabaseServer] ds
                on ds.DatabaseServerID = d.DatabaseServerID
            where 
                u.Auth0ID = @Auth0ID
                and d.DatabaseTypeID = @DatabaseTypeID",
            new
            {
                Auth0ID = new DbString
                {
                    Value = auth0Id,
                    IsFixedLength = false,
                    IsAnsi = true,
                    Length = 50,
                },
                databaseTypeID = databaseTypeId,
            }
        );
    }

    public async Task<List<KeyValuePair<int, string>>> GetDatabaseServers()
    {
        return (
            await locatorDb.QueryAsync<KeyValuePair<int, string>>(
                @$"
                select
                    DatabaseServerID {nameof(KeyValuePair<int, string>.Key)},
                    DatabaseServerName {nameof(KeyValuePair<int, string>.Value)}
                from dbo.DatabaseServer"
            )
        ).ToList();
    }

    public async Task<Client> GetClient(string clientCode)
    {
        return await locatorDb.QuerySingleAsync<Client>(
            @$"
            select
                c.ClientID {nameof(Client.ClientId)},
                c.ClientCode {nameof(Client.ClientCode)},
                c.ClientName {nameof(Client.ClientName)},
                c.ClientStatusID {nameof(Client.ClientStatusId)}
            from dbo.Client c
            inner join dbo.ClientStatus cs
                on c.ClientStatusID = cs.ClientStatusID
            where
                c.ClientCode = @ClientCode",
            new
            {
                ClientCode = new DbString
                {
                    Value = clientCode,
                    IsFixedLength = false,
                    IsAnsi = true,
                    Length = 20,
                },
            }
        );
    }

    public async Task<PagedList<Client>> GetClients(string keyword, int pageNumber, int pageSize)
    {
        var whereClause = keyword == null ? "" : "where c.ClientName like '%' + @Keyword + '%'";
        var results = await locatorDb.QueryMultipleAsync(
            @$"
            select count(*) as TotalCount from dbo.Client c {whereClause}

            select
                c.ClientID {nameof(Client.ClientId)},
                c.ClientCode {nameof(Client.ClientCode)},
                c.ClientName {nameof(Client.ClientName)},
                c.ClientStatusID {nameof(Client.ClientStatusId)}
            from dbo.Client c
            inner join dbo.ClientStatus cs
                on c.ClientStatusID = cs.ClientStatusID
            {whereClause}
            order by
                c.ClientName asc
            offset ((@PageNumber - 1) * @PageSize) rows
            fetch next @PageSize rows only",
            new
            {
                keyword,
                pageNumber,
                pageSize,
            }
        );

        int rowCount = results.Read<int>().First();
        return new(
            results.Read<Client>().ToList(),
            rowCount,
            pageNumber,
            pageSize,
            (int)Math.Ceiling((double)rowCount / pageNumber)
        );
    }

    public async Task<Client> AddClient(AddClient addClient, int createById)
    {
        var clientCode = addClient.ClientName.ToLower().Replace(" ", "");

        await locatorDb.ExecuteAsync(
            @$"
            insert into dbo.Client 
            (
                ClientCode,
                ClientName, 
                ClientStatusID,
                CreateByID,
                ModifyByID
            )
            values 
            (
                @ClientCode,
                @ClientName,
                1,
                @CreateByID,
                @CreateByID
            )",
            new
            {
                clientCode,
                addClient.ClientName,
                createByID = createById,
            }
        );

        return await GetClient(clientCode);
    }

    public async Task<bool> IsClientActive(string clientCode)
    {
        return await locatorDb.QueryFirstAsync<bool>(
            @$"
            if exists(
                select top 1 
                    cc.ClientConnectionID
                from
                    ClientConnection cc 
                inner join [Database] d
                    on d.DatabaseID = cc.DatabaseID
                    and d.IsActive = 1
                where 
                    ClientID = (
                        select 
                            ClientID
                        from dbo.Client
                        where
                            ClientCode = @ClientCode
                    )
            )
                select 1
            else
                select 0",
            new
            {
                ClientCode = new DbString
                {
                    Value = clientCode,
                    IsFixedLength = false,
                    IsAnsi = true,
                    Length = clientCode.Length,
                },
            }
        );
    }

    public async Task<User> GetUser(int userId)
    {
        var user = await locatorDb.QueryAsync<User, int, User>(
            @$"
            select 
                u.Auth0ID {nameof(User.Auth0Id)},
                u.UserID {nameof(User.UserId)},
                u.FirstName {nameof(User.FirstName)},
                u.LastName {nameof(User.LastName)},
                u.EmailAddress {nameof(User.EmailAddress)},
                u.UserStatusID {nameof(User.UserStatusId)},
                c.ClientID {nameof(User.ClientId)},
                r.RoleID
            from dbo.[User] u
            inner join dbo.UserStatus us
                on us.UserStatusID = u.UserStatusID
            inner join dbo.Client c
                on c.ClientID = u.ClientID
            inner join dbo.UserRole ur
                on ur.UserID = u.UserID
            inner join dbo.Role r
                on r.RoleID = ur.RoleID
            where
                u.UserID = @UserID",
            (user, roleId) =>
            {
                user.RoleIds ??= [];
                user.RoleIds.Add(roleId);
                return user;
            },
            new { userID = userId },
            splitOn: "RoleID"
        );

        return user.GroupBy(u => u.UserId)
            .Select(g =>
            {
                var groupedUser = g.First();
                groupedUser.RoleIds = g.Select(u => u.RoleIds.Single()).ToList();
                return groupedUser;
            })
            .Single();
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
                Auth0ID = @Auth0ID",
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

    public async Task<List<string>> GetAuth0Ids(string clientCode)
    {
        return (List<string>)
            await locatorDb.QueryAsync<string>(
                @$"
                select
                    u.Auth0ID {nameof(User.Auth0Id)}
                from dbo.[User] u
                inner join dbo.Client c
                    on c.ClientID = u.ClientID
                where
                    c.ClientCode = @ClientCode",
                new { clientCode }
            );
    }

    public async Task<User> UpdateProfile(
        UpdateProfile updateProfile,
        string auth0Id,
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
                ModifyByID = @ModifyByID,
                ModifyDate = getdate()
            where
                Auth0ID = @Auth0ID",
            new
            {
                updateProfile.FirstName,
                updateProfile.LastName,
                updateProfile.EmailAddress,
                auth0ID = auth0Id,
                modifyByID = modifyById,
            }
        );

        return await GetUser(auth0Id);
    }

    public async Task<User> UpdateUser(UpdateUser updateUser, int modifyById)
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
                ModifyDate = getdate()
            where
                UserID = @UserID",
            new
            {
                updateUser.FirstName,
                updateUser.LastName,
                updateUser.EmailAddress,
                updateUser.UserStatusId,
                updateUser.ClientId,
                updateUser.UserId,
                modifyByID = modifyById,
            }
        );

        await locatorDb.ExecuteAsync(
            @$"
            delete from dbo.UserRole
            where
                UserID = @UserID",
            new { updateUser.UserId }
        );

        foreach (var roleID in updateUser.RoleIds)
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
                    updateUser.UserId,
                    roleID,
                    modifyByID = modifyById,
                }
            );
        }

        return await GetUser(updateUser.UserId);
    }

    public async Task UpdateClient(UpdateClient updateClient, int modifyById)
    {
        await locatorDb.ExecuteAsync(
            @$"
            update [Client]
            set
                ClientStatusID = @ClientStatusID,
                ModifyByID = @ModifyByID,
                ModifyDate = getdate()
            where
                ClientCode = @ClientCode",
            new
            {
                ClientCode = new DbString
                {
                    Value = updateClient.ClientCode,
                    IsFixedLength = false,
                    IsAnsi = true,
                    Length = 20,
                },
                updateClient.ClientStatusId,
                modifyByID = modifyById,
            }
        );
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

    public async Task<ClientUser> GetClientUser(string auth0Id)
    {
        return await locatorDb.QuerySingleAsync<ClientUser>(
            @$"
            select 
                u.FirstName {nameof(ClientUser.FirstName)},
                u.LastName {nameof(ClientUser.LastName)},
                u.EmailAddress {nameof(ClientUser.EmailAddress)},
                c.ClientName {nameof(ClientUser.ClientName)},
                c.ClientCode {nameof(ClientUser.ClientCode)}
            from dbo.[User] u 
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

    public async Task<List<KeyValuePair<int, string>>> GetClientList()
    {
        return (
            await locatorDb.QueryAsync<KeyValuePair<int, string>>(
                @$"
                select
                    ClientID {nameof(KeyValuePair<int, string>.Key)},
                    ClientName {nameof(KeyValuePair<int, string>.Value)}
                from dbo.Client"
            )
        ).ToList();
    }

    public async Task<List<KeyValuePair<int, string>>> GetRoleList()
    {
        return (
            await locatorDb.QueryAsync<KeyValuePair<int, string>>(
                @$"
                select
                    RoleID {nameof(KeyValuePair<int, string>.Key)},
                    Name {nameof(KeyValuePair<int, string>.Value)}
                from dbo.Role"
            )
        ).ToList();
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

    public async Task<User> DeleteUser(string auth0Id)
    {
        await locatorDb.ExecuteAsync(
            @$"
            delete from dbo.UserRole
            where
                UserID = (
                    select 
                        UserID
                    from dbo.[User]
                    where
                        Auth0ID = @Auth0ID
                )

            delete from dbo.[User]
            where
                Auth0ID = @Auth0ID",
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

        return new User();
    }

    public async Task<User> AddUser(AddUser addUser, string auth0Id, int createById)
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
                UserGuid,
                CreateByID,
                ModifyByID
            )
            values
            (
                @ClientID,
                @FirstName,
                @LastName,
                @EmailAddress,
                2,
                @Auth0ID,
                newid(),
                @CreateByID,
                @CreateByID
            )

            select scope_identity() as UserId",
            new
            {
                addUser.ClientId,
                addUser.FirstName,
                addUser.LastName,
                addUser.EmailAddress,
                auth0ID = auth0Id,
                createByID = createById,
            }
        );

        foreach (var roleId in addUser.RoleIds)
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

        return await GetUser(userId);
    }

    public async Task<List<Database>> GetDatabases(string clientCode)
    {
        return (List<Database>)
            await locatorDb.QueryAsync<Database>(
                @$"
                select 
                    d.[DatabaseName] {nameof(Database.DatabaseName)},
                    dt.DatabaseTypeName {nameof(Database.DatabaseType)},
                    ds.DatabaseServerName {nameof(Database.DatabaseServer)},
                    d.IsActive {nameof(Database.IsActive)}
                from ClientConnection cc 
                inner join [Database] d
                    on d.DatabaseID = cc.DatabaseID
                inner join DatabaseType dt  
                    on d.DatabaseTypeID = dt.DatabaseTypeID
                inner join DatabaseServer ds  
                    on d.DatabaseServerID = ds.DatabaseServerID
                where 
                    ClientID = (
                        select 
                            ClientID
                        from Client
                        where
                            ClientCode = @ClientCode
                )",
                new { clientCode }
            );
    }

    public async Task<string> GetUserEmail(int userId)
    {
        return await locatorDb.QuerySingleAsync<string>(
            @$"
            select
                EmailAddress {nameof(User.EmailAddress)}
            from dbo.[User]
            where
                UserID = @UserID",
            new { userId }
        );
    }

    public async Task<string> GetClientCode(int clientId)
    {
        return await locatorDb.QuerySingleAsync<string>(
            @$"
            select
                ClientCode {nameof(Client.ClientCode)}
            from dbo.Client
            where
                ClientID = @ClientID",
            new { clientId }
        );
    }

    public async Task<bool> CheckEmailExists(string emailAddress)
    {
        return await locatorDb.QuerySingleAsync<bool>(
            @$"
            select
                case when exists
                (
                    select 1
                    from dbo.[User]
                    where
                        EmailAddress = @EmailAddress
                )
                then 1
                else 0
                end",
            new
            {
                EmailAddress = new DbString
                {
                    Value = emailAddress,
                    IsFixedLength = false,
                    IsAnsi = true,
                    Length = 100,
                },
            }
        );
    }
}
