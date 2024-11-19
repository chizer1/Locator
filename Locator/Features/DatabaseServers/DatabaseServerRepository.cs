using System.Data;
using Dapper;
using Locator.Domain;
using Locator.Features.Databases;

namespace Locator.Features.DatabaseServers;

internal class DatabaseServerRepository(IDbConnection locatorDb) : IDatabaseServerRepository
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
        return await locatorDb.QuerySingleAsync<DatabaseServer>(
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
            new { databaseServerId }
        );
    }

    public async Task<List<DatabaseServer>> GetDatabaseServers()
    {
        var results = await locatorDb.QueryAsync<DatabaseServer>(
            @$"
            select
                ds.DatabaseServerID {nameof(DatabaseServer.DatabaseServerId)},
                ds.DatabaseServerName {nameof(DatabaseServer.DatabaseServerName)},
                ds.DatabaseServerIPAddress {nameof(DatabaseServer.DatabaseServerIpAddress)}
            from dbo.DatabaseServer ds
            left join dbo.[Database] d
                on ds.DatabaseServerID = d.DatabaseServerID"
        );

        return results.ToList();
    }

    public async Task UpdateDatabaseServer(
        int databaseServerId,
        string databaseServerName,
        string databaseServerIpAddress
    )
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
                databaseServerId,
                databaseServerName,
                databaseServerIpAddress,
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
