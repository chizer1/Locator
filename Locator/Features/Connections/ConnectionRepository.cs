using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Locator.Features.Connections;

internal class ConnectionRepository(IDbConnection locatorDb) : IConnectionRepository
{
    public async Task<int> AddConnection(int clientUserId, int databaseId)
    {
        return await locatorDb.QuerySingleAsync<int>(
            @$"
            insert into dbo.Connection 
            (
                ClientUserID,
                DatabaseID
            )
            values 
            (
                @ClientUserID,
                @DatabaseID
            )

            select scope_identity()",
            new { clientUserId, databaseId }
        );
    }

    public async Task<SqlConnection> GetConnection(string auth0Id, int clientId, int databaseTypeId)
    {
        var connection = await locatorDb.QuerySingleAsync(
            @$"
            select
                dbs.DatabaseServerName,
                db.DatabaseName,
                db.DatabaseUser
            from [User] u
            inner join ClientUser cu
                on u.UserID = cu.UserID
            inner join Connection con 
                on con.ClientUserID = cu.ClientUserID
            inner join [Database] db
                on db.DatabaseID = con.DatabaseID
            inner join [DatabaseServer] dbs
                on dbs.DatabaseServerID = db.DatabaseServerID
            where 
                u.Auth0ID = @Auth0ID
                and cu.ClientID = @ClientID
                and db.DatabaseTypeID = @DatabaseTypeID",
            new
            {
                auth0Id,
                clientId,
                databaseTypeId,
            }
        );

        return new SqlConnection(
            $@"
            Server={connection.DatabaseServerName};
            User Id={connection.DatabaseUser};
            Password=Skyline-Armory-Paramount3-Shut;
            Database={connection.DatabaseName};"
        // change to trusted connection for windows auth or kerberos, and no password
        );
    }

    public Task DeleteConnection(int connectionId)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteConnection(int clientUserId, int databaseId)
    {
        await locatorDb.ExecuteAsync(
            @$"
            delete from dbo.Connection
            where
                ClientUserID = @ClientID
                and DatabaseID = @DatabaseID",
            new { clientUserId, databaseId }
        );
    }
}
