using System.Data;
using Dapper;
using Locator.Models;

namespace Locator.Repositories;

internal class DatabaseRepository(IDbConnection locatorDb)
{
    public async Task<int> AddDatabaseServer(
        string databaseServerName,
        string ipAddress,
        int userId
    )
    {
        return await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.DatabaseServer
            (
                DatabaseServerName,
                IPAddress,
                CreateByID,
                ModifyByID
            )
            values
            (
                @DatabaseServerName,
                @IPAddress,
                @UserID,
                @UserID
            )

            select scope_identity()",
            new
            {
                databaseServerName,
                ipAddress,
                userId,
            }
        );
    }

    public async Task<int> AddDatabase(
        string databaseName,
        string databaseUser,
        string databaseUserPassword,
        int databaseServerId,
        int databaseTypeId,
        int userId
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
                userId,
            }
        );
    }

    public async Task<int> AddDatabaseType(string databaseTypeName, int userId)
    {
        return await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.DatabaseType
            (
                DatabaseTypeName,
                CreateByID,
                ModifyByID
            )
            values
            (
                @DatabaseTypeName,
                @UserID,
                @UserID
            )

            select scope_identity()",
            new { databaseTypeName, userId }
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

    public async Task UpdateDatabaseType(int databaseTypeId, string databaseTypeName, int userId)
    {
        await locatorDb.ExecuteAsync(
            @$"
            update dbo.DatabaseType
            set
                DatabaseTypeName = @DatabaseTypeName,
                ModifyByID = @UserID
            where
                DatabaseTypeID = @DatabaseTypeID",
            new
            {
                databaseTypeId,
                databaseTypeName,
                userId,
            }
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

    public async Task<List<DatabaseServer>> GetDatabaseServers()
    {
        var results = await locatorDb.QueryAsync<DatabaseServer, Database, DatabaseServer>(
            @$"
            select
                ds.DatabaseServerID {nameof(DatabaseServer.DatabaseServerId)},
                ds.DatabaseServerName {nameof(DatabaseServer.DatabaseServerName)},
                ds.IpAddress {nameof(DatabaseServer.DatabaseServerIpAddress)},
                d.DatabaseID,
                d.DatabaseName {nameof(Database.DatabaseName)},
                d.DatabaseServerID {nameof(Database.DatabaseServerId)},
                d.DatabaseTypeID {nameof(Database.DatabaseTypeId)}
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

    // get databases
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
}
