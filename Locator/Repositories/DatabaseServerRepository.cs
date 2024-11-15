using System.Data;
using Dapper;
using Locator.Domain;
using Locator.Models.Read;
using Locator.Models.Write;

namespace Locator.Repositories;

internal class DatabaseServerRepository(IDbConnection locatorDb)
{
    public async Task<int> AddDatabaseServer(AddDatabaseServer addDatabaseServer)
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
            new { addDatabaseServer.DatabaseServerName, addDatabaseServer.DatabaseServerIpAddress }
        );
    }

    public async Task<DatabaseServer> GetDatabaseServer(int databaseServerId)
    {
        var results = await locatorDb.QueryAsync<DatabaseServer, Database, DatabaseServer>(
            @$"
            select
                ds.DatabaseServerID {nameof(DatabaseServer.DatabaseServerId)},
                ds.DatabaseServerName {nameof(DatabaseServer.DatabaseServerName)},
                ds.DatabaseServerIPAddress {nameof(DatabaseServer.DatabaseServerIpAddress)},
                d.DatabaseID {nameof(Database.DatabaseId)},
                d.DatabaseName {nameof(Database.DatabaseName)}
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

        return results
            .GroupBy(ds => ds.DatabaseServerId)
            .Select(g =>
            {
                var databaseServer = g.First();
                databaseServer.Databases = g.Select(ds => ds.Databases.FirstOrDefault())
                    .Where(d => d != null)
                    .ToList();

                return databaseServer;
            })
            .FirstOrDefault();
    }

    public async Task<List<DatabaseServer>> GetDatabaseServers()
    {
        var results = await locatorDb.QueryAsync<DatabaseServer, Database, DatabaseServer>(
            @$"
            select
                ds.DatabaseServerID {nameof(DatabaseServer.DatabaseServerId)},
                ds.DatabaseServerName {nameof(DatabaseServer.DatabaseServerName)},
                ds.DatabaseServerIPAddress {nameof(DatabaseServer.DatabaseServerIpAddress)},
                d.DatabaseID {nameof(Database.DatabaseId)},
                d.DatabaseName {nameof(Database.DatabaseName)}
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

        return results
            .GroupBy(ds => ds.DatabaseServerId)
            .Select(g =>
            {
                var databaseServer = g.First();
                databaseServer.Databases = g.Select(ds => ds.Databases.FirstOrDefault())
                    .Where(d => d != null)
                    .ToList();

                return databaseServer;
            })
            .ToList();
    }

    public async Task UpdateDatabaseServer(UpdateDatabaseServer updateDatabaseServer)
    {
        await locatorDb.ExecuteAsync(
            @$"
            update dbo.DatabaseServer
            set
                DatabaseServerName = @DatabaseServerName,
                DatabaseServerIPAddress = @DatabaseServerIpAddress
            where
                DatabaseServerID = @DatabaseServerID",
            new
            {
                updateDatabaseServer.DatabaseServerId,
                updateDatabaseServer.DatabaseServerName,
                updateDatabaseServer.DatabaseServerIpAddress,
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
