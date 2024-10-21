using System.Data.SqlClient;
using Dapper;
using Locator.Models;

namespace Locator;

public class DatabaseHelper
{
    public static SqlConnection CreateSqlConnection(Connection connection)
    {
        return new SqlConnection(
            $@"
            Server={connection.DatabaseServer};
            User Id={connection.DatabaseUser};
            Password={connection.DatabaseUserPassword};
            Database={connection.DatabaseName};"
        );
    }

    public static SqlConnection CreateClientConnection(
        string clientCode,
        string locatorDbConnectionString,
        DatabaseType databaseType
    )
    {
        using var locatorDb = CreateLocatorConnection(locatorDbConnectionString);

        return CreateSqlConnection(
            locatorDb.QuerySingle<Connection>(
                $@"
                select
                    ds.[DatabaseServerName] as {nameof(Connection.DatabaseServer)},
                    d.[DatabaseName] as {nameof(Connection.DatabaseName)},
                    d.[DatabaseUser] as {nameof(Connection.DatabaseUser)},
                    d.[DatabaseUserPassword] as {nameof(Connection.DatabaseUserPassword)}
                from [dbo].[Client] c
                inner join [dbo].[ClientConnection] cc  
                    on cc.ClientID = c.ClientID
                inner join [dbo].[Database] d
                    on d.DatabaseID = cc.DatabaseID
                inner join [dbo].[DatabaseServer] ds
                    on ds.DatabaseServerID = d.DatabaseServerID
                where 
                    c.ClientCode = @ClientCode
                    and d.DatabaseTypeID = @DatabaseTypeID",
                new { ClientCode = clientCode, DatabaseTypeID = (int)databaseType }
            )
        );
    }

    public static SqlConnection CreateLocatorConnection(string locatorDbConnectionString)
    {
        return new SqlConnection(locatorDbConnectionString);
    }
}
