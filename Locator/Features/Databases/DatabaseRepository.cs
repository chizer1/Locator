using System.Data;
using Dapper;
using Locator.Common.Models;
using Locator.Domain;

namespace Locator.Features.Databases;

internal class DatabaseRepository(IDbConnection locatorDb)
{
    public async Task<int> AddDatabase(
        string databaseName,
        string databaseUser,
        int databaseServerId,
        int databaseTypeId,
        DatabaseStatus databaseStatus
    )
    {
        var databaseId = await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.[Database]
            (
                DatabaseName,
                DatabaseUser,
                DatabaseServerID,
                DatabaseTypeID,
                DatabaseStatusID
            )
            values
            (
                @DatabaseName,
                @DatabaseUser,
                @DatabaseServerID,
                @DatabaseTypeID,
                @DatabaseStatusID
            )

            select scope_identity()",
            new
            {
                databaseName,
                databaseUser,
                databaseServerId,
                databaseTypeId,
                DatabaseStatusID = (int)databaseStatus,
            }
        );

        await locatorDb.ExecuteAsync(
            $"create login {databaseUser} with password = 'Skyline-Armory-Paramount3-Shut'"
        );

        await locatorDb.ExecuteAsync($"create database {databaseName}");

        await locatorDb.ExecuteAsync(
            @$"
            use {databaseName}
            create user {databaseUser} for login {databaseUser}"
        );

        // need other grants / permissions here?
        await locatorDb.ExecuteAsync(
            @$"
            use {databaseName}
            grant select to {databaseUser}"
        );

        return databaseId;
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
                    dt.DatabaseTypeID {nameof(DatabaseType.DatabaseTypeId)}
                from dbo.[Database] d
                join dbo.DatabaseServer ds 
                    on d.DatabaseServerID = ds.DatabaseServerID
                join dbo.DatabaseType dt 
                    on d.DatabaseTypeID = dt.DatabaseTypeID"
            );
    }

    public async Task<PagedList<Database>> GetDatabases(
        string keyword,
        int pageNumber,
        int pageSize
    )
    {
        var whereClause = string.IsNullOrWhiteSpace(keyword)
            ? ""
            : "where d.DatabaseName like @Keyword";

        var sql =
            @$"
            select
                d.DatabaseID {nameof(Database.DatabaseId)},
                d.DatabaseName {nameof(Database.DatabaseName)},
                ds.DatabaseServerID {nameof(DatabaseServer.DatabaseServerId)},
                dt.DatabaseTypeID {nameof(DatabaseType.DatabaseTypeId)}
            from dbo.[Database] d
            join dbo.DatabaseServer ds 
                on d.DatabaseServerID = ds.DatabaseServerID
            join dbo.DatabaseType dt 
                on d.DatabaseTypeID = dt.DatabaseTypeID
            {whereClause}
            order by
                d.DatabaseName
            offset
                @Offset rows
            fetch next
                @PageSize rows only";

        var results = await locatorDb.QueryMultipleAsync(
            @$"
            select count(*) as TotalCount from dbo.[Database] d {whereClause}

            {sql}",
            new
            {
                keyword,
                Offset = (pageNumber - 1) * pageSize,
                PageSize = pageSize,
            }
        );

        var rowCount = results.Read<int>().First();
        return new PagedList<Database>(
            results.Read<Database>().ToList(),
            rowCount,
            pageNumber,
            pageSize,
            (int)Math.Ceiling((double)rowCount / pageSize)
        );
    }

    private async Task<Database> GetDatabase(int databaseId)
    {
        var results = await locatorDb.QueryAsync<Database, DatabaseServer, DatabaseType, Database>(
            @$"
            select
                d.DatabaseID {nameof(Database.DatabaseId)},
                d.DatabaseName {nameof(Database.DatabaseName)},
                d.DatabaseUser {nameof(Database.DatabaseUserName)},
                ds.DatabaseServerID {nameof(DatabaseServer.DatabaseServerId)},
                ds.DatabaseServerName {nameof(DatabaseServer.DatabaseServerName)},
                ds.DatabaseServerIpAddress {nameof(DatabaseServer.DatabaseServerIpAddress)},
                dt.DatabaseTypeID {nameof(DatabaseType.DatabaseTypeId)},
                dt.DatabaseTypeName {nameof(DatabaseType.DatabaseTypeName)}
            from dbo.[Database] d
            join dbo.DatabaseServer ds 
                on d.DatabaseServerID = ds.DatabaseServerID
            join dbo.DatabaseType dt 
                on d.DatabaseTypeID = dt.DatabaseTypeID
            where
                d.DatabaseID = @DatabaseID",
            (database, databaseServer, databaseType) =>
            {
                //database.DatabaseServer = databaseServer;
                database.DatabaseType = databaseType;
                return database;
            },
            new { databaseId },
            splitOn: $"DatabaseServerID, DatabaseTypeID"
        );

        return results.FirstOrDefault();
    }

    public async Task UpdateDatabase(
        int databaseId,
        string databaseName,
        string databaseUserName,
        int databaseServerId,
        int databaseTypeId,
        DatabaseStatus databaseStatus
    )
    {
        var database = await GetDatabase(databaseId);

        await locatorDb.ExecuteAsync(
            @$"
            update dbo.[Database]
            set
                DatabaseName = @DatabaseName,
                DatabaseUser = @DatabaseUser,
                DatabaseServerID = @DatabaseServerID,
                DatabaseTypeID = @DatabaseTypeID,
                DatabaseStatusID = @DatabaseStatusID
            where
                DatabaseID = @DatabaseID",
            new
            {
                databaseId,
                databaseName,
                databaseUserName,
                databaseServerId,
                databaseTypeId,
                DatabaseStatusID = (int)databaseStatus,
            }
        );

        if (database.DatabaseName != databaseName)
            await locatorDb.ExecuteAsync(
                $"alter database {database.DatabaseName} modify name = {databaseName}"
            );

        if (database.DatabaseUserName != databaseUserName)
        {
            await locatorDb.ExecuteAsync(
                @$"
                use {databaseName}
                alter user {database.DatabaseUserName} with name = {databaseUserName}"
            );
        }
    }

    public async Task DeleteDatabase(int databaseId)
    {
        var database = await GetDatabase(databaseId);

        await locatorDb.ExecuteAsync($"drop database {database.DatabaseName}");
        await locatorDb.ExecuteAsync($"drop login {database.DatabaseUserName}");

        await locatorDb.ExecuteAsync(
            @$"
            delete from dbo.[Database]
            where
                DatabaseID = @DatabaseID",
            new { database.DatabaseId }
        );
    }
}
