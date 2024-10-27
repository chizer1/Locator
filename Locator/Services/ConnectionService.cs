using System.Data.SqlClient;
using Locator.Models;
using Locator.Repositories;

namespace Locator.Services;

internal class ConnectionService(ConnectionRepository connectionRepository)
{
    public async Task<int> AddConnection(int clientId, int databaseId)
    {
        return await connectionRepository.AddConnection(clientId, databaseId);
    }

    public async Task<Connection> GetConnection(int connectionId)
    {
        return await connectionRepository.GetConnection(connectionId);
    }

    public async Task<SqlConnection> GetConnection(string auth0Id, int clientId, int databaseTypeId)
    {
        return await connectionRepository.GetConnection(auth0Id, clientId, databaseTypeId);
    }

    public async Task DeleteConnection(int connectionId)
    {
        await connectionRepository.DeleteConnection(connectionId);
    }
}
