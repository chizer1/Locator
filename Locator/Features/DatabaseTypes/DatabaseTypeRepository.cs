using System.Data;
using Dapper;
using Locator.Domain;

namespace Locator.Features.DatabaseTypes;

internal class DatabaseTypeRepository(IDbConnection locatorDb)
{
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

    public async Task<DatabaseType> GetDatabaseType(int databaseTypeId)
    {
        return await locatorDb.QueryFirstOrDefaultAsync<DatabaseType>(
            @$"
            select
                DatabaseTypeID {nameof(DatabaseType.DatabaseTypeId)},
                DatabaseTypeName {nameof(DatabaseType.DatabaseTypeName)}
            from dbo.DatabaseType
            where
                DatabaseTypeID = @DatabaseTypeID",
            new { databaseTypeId }
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

    public async Task DeleteDatabaseType(int databaseTypeId)
    {
        await locatorDb.ExecuteAsync(
            @$"
            delete from dbo.DatabaseType
            where
                DatabaseTypeID = @DatabaseTypeID",
            new { databaseTypeId }
        );
    }
}
