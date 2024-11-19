using System.Data.SqlClient;
using Locator.Features.Connections;
using Locator.Features.Connections.AddConnection;
using Locator.Features.Connections.DeleteConnection;
using Locator.Features.Connections.GetConnection;

namespace Locator.Library;

public class Connections
{
    private readonly AddConnection _addConnection;
    private readonly GetConnection _getConnection;
    private readonly DeleteConnection _deleteConnection;

    public Connections(SqlConnection locatorDb)
    {
        IConnectionRepository connectionRepository = new ConnectionRepository(locatorDb);

        _addConnection = new AddConnection(connectionRepository);
        _getConnection = new GetConnection(connectionRepository);
        _deleteConnection = new DeleteConnection(connectionRepository);
    }

    public async Task<int> AddConnection(int clientUserId, int databaseId)
    {
        return await _addConnection.Handle(new AddConnectionCommand(clientUserId, databaseId));
    }

    public async Task<SqlConnection> GetConnection(string auth0, int clientId, int databaseTypeId)
    {
        return await _getConnection.Handle(new GetConnectionQuery(auth0, clientId, databaseTypeId));
    }

    public async Task DeleteConnection(int connectionId)
    {
        await _deleteConnection.Handle(new DeleteConnectionCommand(connectionId));
    }
}
