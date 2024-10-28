using System.Data;
using Dapper;
using Locator.Models;

namespace Locator.Repositories;

internal class DatabaseServerRepository(IDbConnection locatorDb)
{
    public async Task<int> AddDatabaseServer(
        string databaseServerName,
        string databaseServerIpAddress
    )
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
                @DatabaseServerIpAddress
            )

            select scope_identity()",
            new { databaseServerName, databaseServerIpAddress }
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
}
