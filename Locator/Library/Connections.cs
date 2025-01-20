using System.Data.SqlClient;
using Locator.Features.Clients;
using Locator.Features.Connections;
using Locator.Features.Connections.AddConnection;
using Locator.Features.Connections.DeleteConnection;
using Locator.Features.Connections.GetConnection;
using Locator.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace Locator.Library;

internal class Connections
{
    private readonly AddConnection _addConnection;
    private readonly GetConnection _getConnection;
    private readonly DeleteConnection _deleteConnection;

    public Connections(DbContext locatorDb)
    {
        IConnectionRepository connectionRepository = new ConnectionRepository(locatorDb);
        IClientRepository clientRepository = new ClientRepository(locatorDb);
        IUserRepository userRepository = new UserRepository(locatorDb);

        _addConnection = new AddConnection(connectionRepository);
        _getConnection = new GetConnection(connectionRepository, clientRepository, userRepository);
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
