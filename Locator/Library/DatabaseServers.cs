using System.Data.SqlClient;
using Locator.Domain;
using Locator.Features.DatabaseServers;
using Locator.Features.DatabaseServers.AddDatabaseServer;
using Locator.Features.DatabaseServers.DeleteDatabaseServer;
using Locator.Features.DatabaseServers.GetDatabaseServers;
using Locator.Features.DatabaseServers.UpdateDatabaseServer;

namespace Locator.Library;

public class DatabaseServers
{
    private readonly AddDatabaseServer _addDatabaseServer;
    private readonly GetDatabaseServers _getDatabaseServers;
    private readonly UpdateDatabaseServer _updateDatabaseServer;
    private readonly DeleteDatabaseServer _deleteDatabaseServer;

    public DatabaseServers(SqlConnection locatorDb)
    {
        IDatabaseServerRepository databaseServerRepository = new DatabaseServerRepository(
            locatorDb
        );

        _addDatabaseServer = new AddDatabaseServer(databaseServerRepository);
        _getDatabaseServers = new GetDatabaseServers(databaseServerRepository);
        _updateDatabaseServer = new UpdateDatabaseServer(databaseServerRepository);
        _deleteDatabaseServer = new DeleteDatabaseServer(databaseServerRepository);
    }

    public async Task<int> AddDatabaseServer(
        string databaseServerName,
        string databaseServerIpAddress
    )
    {
        return await _addDatabaseServer.Handle(
            new AddDatabaseServerCommand(databaseServerName, databaseServerIpAddress)
        );
    }

    public async Task<List<DatabaseServer>> GetDatabaseServers()
    {
        return await _getDatabaseServers.Handle(new GetDatabaseServersQuery());
    }

    public async Task UpdateDatabaseServer(
        int databaseServerId,
        string databaseServerName,
        string databaseServerIpAddress
    )
    {
        await _updateDatabaseServer.Handle(
            new UpdateDatabaseServerCommand(
                databaseServerId,
                databaseServerName,
                databaseServerIpAddress
            )
        );
    }

    public async Task DeleteDatabaseServer(int databaseServerId)
    {
        await _deleteDatabaseServer.Handle(new DeleteDatabaseServerCommand(databaseServerId));
    }
}
