using System.Data;
using Dapper;
using Locator.Models;

namespace Locator.Repositories;

internal class LocatorRepository(IDbConnection locatorDb)
{
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

    public async Task UpdateDatabaseStatus(int databaseId, bool isActive)
    {
        await locatorDb.ExecuteAsync(
            @$"
            update [Database]
            set
                IsActive = @IsActive
            where
                DatabaseID = @DatabaseID",
            new { databaseId, isActive }
        );
    }

    public async Task AddPermissions(
        string databaseName,
        string databaseUser,
        string databasePassword
    )
    {
        await locatorDb.ExecuteAsync(
            @$"create login {databaseUser} with password = '{databasePassword}'",
            new { databaseUser, databasePassword }
        );

        await locatorDb.ExecuteAsync(
            @$"
                use {databaseName}
                create user {databaseUser} for login {databaseUser}

                alter role [db_datareader] add member {databaseUser}

                alter role [db_datawriter] add member {databaseUser}",
            new { databaseName, databaseUser }
        );
    }

    public async Task AddClientConnection(int clientId, int databaseId, int createById)
    {
        await locatorDb.ExecuteAsync(
            @$"
            insert into dbo.ClientConnection
            (
                ClientID,
                DatabaseID,
                CreateByID,
                ModifyByID
            )
            values
            (
                @ClientID,
                @DatabaseID,
                @CreateByID,
                @CreateByID
            )",
            new
            {
                clientId,
                databaseId,
                createByID = createById,
            }
        );
    }

    public async Task<int> AddDatabaseServer(string databaseServerName, int userID)
    {
        return await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.DatabaseServer
            (
                DatabaseServerName,
                CreateByID,
                ModifyByID
            )
            values
            (
                @DatabaseServerName,
                @UserID,
                @UserID
            )

            select scope_identity()",
            new { databaseServerName, userID }
        );
    }

    public async Task<int> AddDatabase(
        string databaseName,
        string databaseUser,
        string databaseUserPassword,
        int databaseServerId,
        int databaseTypeId,
        int userID
    )
    {
        return await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.[Database]
            (
                DatabaseName,
                DatabaseUser,
                DatabaseUserPassword,
                DatabaseServerID,
                DatabaseTypeID,
                CreateByID,
                ModifyByID
            )
            values
            (
                @DatabaseName,
                @DatabaseUser,
                @DatabaseUserPassword,
                @DatabaseServerID,
                @DatabaseTypeID,
                @UserID,
                @UserID
            )
            
            select scope_identity()",
            new
            {
                databaseName,
                databaseUser,
                databaseUserPassword,
                databaseServerId,
                databaseTypeId,
                userID,
            }
        );
    }

    // public async Task<List<Database>> GetDatabases(string clientCode)
    // {
    //     return (List<Database>)
    //         await locatorDb.QueryAsync<Database>(
    //             @$"
    //             select
    //                 d.[DatabaseName] {nameof(Database.DatabaseName)},
    //                 dt.DatabaseTypeName {nameof(Database.DatabaseType)},
    //                 ds.DatabaseServerName {nameof(Database.DatabaseServer)},
    //                 d.IsActive {nameof(Database.IsActive)}
    //             from ClientConnection cc
    //             inner join [Database] d
    //                 on d.DatabaseID = cc.DatabaseID
    //             inner join DatabaseType dt
    //                 on d.DatabaseTypeID = dt.DatabaseTypeID
    //             inner join DatabaseServer ds
    //                 on d.DatabaseServerID = ds.DatabaseServerID
    //             where
    //                 ClientID = (
    //                     select
    //                         ClientID
    //                     from Client
    //                     where
    //                         ClientCode = @ClientCode
    //             )",
    //             new { clientCode }
    //         );
    // }
}
