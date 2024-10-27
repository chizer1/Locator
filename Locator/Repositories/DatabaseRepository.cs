using System.Data;
using Dapper;
using Locator.Models;

namespace Locator.Repositories;

internal class DatabaseRepository(IDbConnection locatorDb)
{
    #region DatabaseServer

    public async Task<int> AddDatabaseServer(string databaseServerName, string ipAddress)
    {
        return await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.DatabaseServer
            (
                DatabaseServerName,
                DatabaseServerIPAddress
            )
            values
            (
                @DatabaseServerName,
                @IPAddress
            )

            select scope_identity()",
            new { databaseServerName, ipAddress }
        );
    }

    public async Task<DatabaseServer> GetDatabaseServer(int databaseServerId)
    {
        var results = await locatorDb.QueryAsync<DatabaseServer, Database, DatabaseServer>(
            @$"
            select
                ds.DatabaseServerID {nameof(DatabaseServer.DatabaseServerId)},
                ds.DatabaseServerName {nameof(DatabaseServer.DatabaseServerName)},
                ds.IpAddress {nameof(DatabaseServer.DatabaseServerIpAddress)},
                d.DatabaseID {nameof(Database.DatabaseId)},
                d.DatabaseName {nameof(Database.DatabaseName)},
                d.DatabaseServerID {nameof(Database.DatabaseServer)},
                d.DatabaseTypeID {nameof(Database.DatabaseType)}
            from dbo.DatabaseServer ds
            left join dbo.[Database] d
                on ds.DatabaseServerID = d.DatabaseServerID
            where
                ds.DatabaseServerID = @DatabaseServerID",
            (ds, d) =>
            {
                ds.Databases ??= [];

                if (d != null)
                    ds.Databases.Add(d);

                return ds;
            },
            new { databaseServerId },
            splitOn: $"{nameof(Database.DatabaseId)}"
        );

        return results.FirstOrDefault();
    }

    public async Task<List<DatabaseServer>> GetDatabaseServers()
    {
        var results = await locatorDb.QueryAsync<DatabaseServer, Database, DatabaseServer>(
            @$"
            select
                ds.DatabaseServerID {nameof(DatabaseServer.DatabaseServerId)},
                ds.DatabaseServerName {nameof(DatabaseServer.DatabaseServerName)},
                ds.IpAddress {nameof(DatabaseServer.DatabaseServerIpAddress)},
                d.DatabaseID {nameof(Database.DatabaseId)},
                d.DatabaseName {nameof(Database.DatabaseName)},
                d.DatabaseServerID {nameof(Database.DatabaseServer)},
                d.DatabaseTypeID {nameof(Database.DatabaseType)}
            from dbo.DatabaseServer ds
            left join dbo.[Database] d
                on ds.DatabaseServerID = d.DatabaseServerID",
            (ds, d) =>
            {
                ds.Databases ??= [];

                if (d != null)
                    ds.Databases.Add(d);

                return ds;
            },
            splitOn: $"{nameof(Database.DatabaseId)}"
        );

        return results.ToList();
    }

    public async Task UpdateDatabaseServer(
        int databaseServerId,
        string databaseServerName,
        string ipAddress
    )
    {
        await locatorDb.ExecuteAsync(
            @$"
            update dbo.DatabaseServer
            set
                DatabaseServerName = @DatabaseServerName,
                DatabaseServerIPAddress = @IPAddress
            where
                DatabaseServerID = @DatabaseServerID",
            new
            {
                databaseServerId,
                databaseServerName,
                ipAddress,
            }
        );
    }

    public async Task DeleteDatabaseServer(int databaseServerId)
    {
        await locatorDb.ExecuteAsync(
            @$"
            delete from dbo.DatabaseServer
            where
                DatabaseServerID = @DatabaseServerID",
            new { databaseServerId }
        );
    }

    #endregion

    #region Database

    public async Task<int> AddDatabase(
        string databaseName,
        string databaseUser,
        string databaseUserPassword,
        int databaseServerId,
        int databaseTypeId,
        DatabaseStatus databaseStatus
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
                DatabaseStatusID
            )
            values
            (
                @DatabaseName,
                @DatabaseUser,
                @DatabaseUserPassword,
                @DatabaseServerID,
                @DatabaseTypeID,
                @DatabaseStatusID
            )

            select scope_identity()",
            new
            {
                databaseName,
                databaseUser,
                databaseUserPassword,
                databaseServerId,
                databaseTypeId,
                DatabaseStatusID = (int)databaseStatus,
            }
        );
    }

    public async Task<List<Database>> GetDatabases()
    {
        return (List<Database>)
            await locatorDb.QueryAsync<Database>(
                @$"
                select
                    d.DatabaseID {nameof(Database.DatabaseId)},
                    d.DatabaseName {nameof(Database.DatabaseName)},
                    ds.DatabaseServerID {nameof(DatabaseServer.DatabaseServerId)},
                    dt.DatabaseTypeID {nameof(DatabaseType.DatabaseTypeId)},
                from dbo.[Database] d
                join dbo.DatabaseServer ds 
                    on d.DatabaseServerID = ds.DatabaseServerID
                join dbo.DatabaseType dt 
                    on d.DatabaseTypeID = dt.DatabaseTypeID"
            );
    }

    #endregion

    #region DatabaseType

    public async Task<int> AddDatabaseType(string databaseTypeName)
    {
        return await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.DatabaseType
            (
                DatabaseTypeName
            )
            values
            (
                @DatabaseTypeName
            )

            select scope_identity()",
            new { databaseTypeName }
        );
    }

    public async Task<List<DatabaseType>> GetDatabaseTypes()
    {
        return (List<DatabaseType>)
            await locatorDb.QueryAsync<DatabaseType>(
                @$"
                select
                    DatabaseTypeID {nameof(DatabaseType.DatabaseTypeId)},
                    DatabaseTypeName {nameof(DatabaseType.DatabaseTypeName)}
                from dbo.DatabaseType"
            );
    }

    public async Task UpdateDatabaseType(int databaseTypeId, string databaseTypeName)
    {
        await locatorDb.ExecuteAsync(
            @$"
            update dbo.DatabaseType
            set
                DatabaseTypeName = @DatabaseTypeName
            where
                DatabaseTypeID = @DatabaseTypeID",
            new { databaseTypeId, databaseTypeName }
        );
    }

    public async Task RemoveDatabaseType(int databaseTypeId)
    {
        await locatorDb.ExecuteAsync(
            @$"
            delete from dbo.DatabaseType
            where
                DatabaseTypeID = @DatabaseTypeID",
            new { databaseTypeId }
        );
    }

    #endregion
}
