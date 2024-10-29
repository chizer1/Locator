using System.Data;
using System.Data.SqlClient;
using Dapper;
using Locator.Models.Read;

namespace Locator.Repositories;

internal class ConnectionRepository(IDbConnection locatorDb)
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

    public async Task<Connection> GetConnection(int connectionId)
    {
        var results = await locatorDb.QueryAsync<Connection, Database, User, Client, Connection>(
            @$"
            select
                ConnectionID {nameof(Connection.ConnectionId)},
                DatabaseID {nameof(Connection.Database.DatabaseId)},
                ClientID {nameof(Connection.Client.ClientId)}
            from dbo.Connection
            where 
                ConnectionID = @ConnectionID",
            (connection, database, user, client) =>
            {
                connection.Database = database;
                connection.Client = client;
                return connection;
            },
            new { connectionId },
            splitOn: $"{nameof(Database.DatabaseId)}, {nameof(User.UserId)}, ${nameof(Client.ClientId)}"
        );

        return results.FirstOrDefault();
    }

    public async Task<SqlConnection> GetConnection(string auth0Id, int clientId, int databaseTypeId)
    {
        var connection = await locatorDb.QuerySingleAsync(
            @$"
            select
                dbs.DatabaseServerName,
                db.DatabaseName,
                db.DatabaseUser,
                db.DatabaseUserPassword
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
            Password={connection.DatabaseUserPassword};
            Database={connection.DatabaseName};"
        );
    }

    // get connections
    public async Task<List<Connection>> GetConnections()
    {
        var results = await locatorDb.QueryAsync<Connection, Database, User, Client, Connection>(
            @$"
            select
                ConnectionID {nameof(Connection.ConnectionId)},
                DatabaseID {nameof(Connection.Database.DatabaseId)},
                ClientID {nameof(Connection.Client.ClientId)}
            from dbo.Connection",
            (connection, database, user, client) =>
            {
                connection.Database = database;
                connection.Client = client;
                return connection;
            },
            splitOn: $"{nameof(Database.DatabaseId)}, {nameof(User.UserId)}, ${nameof(Client.ClientId)}"
        );

        return results.ToList();
    }

    public async Task DeleteConnection(int connectionId)
    {
        await locatorDb.ExecuteAsync(
            @$"
            delete from dbo.Connection
            where ConnectionID = @ConnectionID",
            new { connectionId }
        );
    }
}
