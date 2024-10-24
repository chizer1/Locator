using System.Data;
using Dapper;
using Locator.Models;

namespace Locator.Repositories;

internal class DatabaseRepository(IDbConnection locatorDb)
{
    public async Task<int> AddDatabaseServer(string databaseServerName, int userId)
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
            new { databaseServerName, userId }
        );
    }

    public async Task<List<DatabaseServer>> GetDatabaseServers()
    {
        return (List<DatabaseServer>)
            await locatorDb.QueryAsync<DatabaseServer>(
                @$"
                select
                    DatabaseServerID {nameof(DatabaseServer.Id)},
                    DatabaseServerName {nameof(DatabaseServer.Name)}
                from dbo.DatabaseServer"
            );
    }
}
