using Locator.Common.Models;
using Locator.Domain;

namespace Locator.Features.Connections;

internal interface IConnectionRepository
{
    public Task<int> AddConnection(int clientUserId, int databaseId);
    public Task<Connection> GetConnection(string auth0Id, int clientId, int databaseTypeId);
    public Task DeleteConnection(int connectionId);
    public Task<PagedList<Connection>> GetConnections(string keyword, int pageNumber, int pageSize);
}
