using System.Data.SqlClient;

namespace Locator.Features.Connections;

public interface IConnectionRepository
{
    public Task<int> AddConnection(int clientUserId, int databaseId);
    public Task<SqlConnection> GetConnection(string auth0Id, int clientId, int databaseTypeId);
    public Task DeleteConnection(int clientId, int userId);
}
