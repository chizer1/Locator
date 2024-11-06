using System.Data;
using Dapper;
using Locator.Models.Read;
using Locator.Models.Write;

namespace Locator.Repositories;

internal class DatabaseRepository(IDbConnection locatorDb)
{
    public async Task<int> AddDatabase(AddDatabase addDatabase)
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
                addDatabase.DatabaseName,
                addDatabase.DatabaseUser,
                addDatabase.DatabaseServerId,
                addDatabase.DatabaseTypeId,
                DatabaseStatusID = (int)addDatabase.DatabaseStatus,
            }
        );

        await locatorDb.ExecuteAsync(
            @$"create login {addDatabase.DatabaseUser} with password = 'Skyline-Armory-Paramount3-Shut'"
        );

        await locatorDb.ExecuteAsync(@$"create database {addDatabase.DatabaseName}");
        await locatorDb.ExecuteAsync(
            @$"create login {addDatabase.DatabaseUser} with password = 'Skyline-Armory-Paramount3-Shut'"
        );
        await locatorDb.ExecuteAsync(
            @$"
            use {addDatabase.DatabaseName}
            create user {addDatabase.DatabaseUser} for login {addDatabase.DatabaseUser}"
        );

        // need other grants / permissions here?
        await locatorDb.ExecuteAsync(
            @$"
            use {addDatabase.DatabaseName}
            grant select to {addDatabase.DatabaseUser}"
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

        int rowCount = results.Read<int>().First();
        return new(
            results.Read<Database>().ToList(),
            rowCount,
            pageNumber,
            pageSize,
            (int)Math.Ceiling((double)rowCount / pageSize)
        );
    }

    public async Task<Database> GetDatabase(int databaseId)
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

    public async Task UpdateDatabase(UpdateDatabase updateDatabase)
    {
        var database = await GetDatabase(updateDatabase.DatabaseId);

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
                updateDatabase.DatabaseId,
                updateDatabase.DatabaseName,
                updateDatabase.DatabaseUserName,
                updateDatabase.DatabaseServerId,
                updateDatabase.DatabaseTypeId,
                DatabaseStatusID = (int)updateDatabase.DatabaseStatus,
            }
        );

        if (database.DatabaseName != updateDatabase.DatabaseName)
            await locatorDb.ExecuteAsync(
                $"alter database {database.DatabaseName} modify name = {updateDatabase.DatabaseName}"
            );

        if (database.DatabaseUserName != updateDatabase.DatabaseUserName)
        {
            await locatorDb.ExecuteAsync(
                @$"
                use {updateDatabase.DatabaseName}
                alter user {database.DatabaseUserName} with name = {updateDatabase.DatabaseUserName}"
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
