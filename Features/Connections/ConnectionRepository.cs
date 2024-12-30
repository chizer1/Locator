using Locator.Common.Models;
using Locator.Db;
using Locator.Domain;
using Microsoft.EntityFrameworkCore;

namespace Locator.Features.Connections;

internal class ConnectionRepository(DbContext locatorDb) : IConnectionRepository
{
    public async Task<int> AddConnection(int clientUserId, int databaseId)
    {
        var connection = new ConnectionEntity
        {
            ClientUserId = clientUserId,
            DatabaseId = databaseId,
        };

        await locatorDb.Set<ConnectionEntity>().AddAsync(connection);
        await locatorDb.SaveChangesAsync();

        return connection.ConnectionId;
    }

    public async Task<Connection> GetConnection(string auth0Id, int clientId, int databaseTypeId)
    {
        var connectionEntity =
            await locatorDb
                .Set<ConnectionEntity>()
                .Include(con => con.ClientUser)
                .ThenInclude(cu => cu.User)
                .Include(con => con.Database)
                .Where(con =>
                    con.ClientUser.User.Auth0Id == auth0Id
                    && con.ClientUser.ClientId == clientId
                    && con.Database.DatabaseTypeId == databaseTypeId
                )
                .FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Connection not found.");

        return new Connection(
            connectionEntity.ConnectionId,
            connectionEntity.DatabaseId,
            connectionEntity.ClientUserId
        );
    }

    public async Task<PagedList<Connection>> GetConnections(
        string keyword,
        int pageNumber,
        int pageSize
    )
    {
        var query = locatorDb.Set<ConnectionEntity>().AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            // Assuming keyword filtering logic applies here if relevant fields exist.
        }

        var totalCount = await query.CountAsync();

        var connectionEntities = await query
            .OrderBy(c => c.ConnectionId) // Adjust sorting as needed.
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var connections = connectionEntities
            .Select(entity => new Connection(
                entity.ConnectionId,
                entity.ClientUserId,
                entity.DatabaseId
            // Add other properties as needed
            ))
            .ToList();

        return new PagedList<Connection>(
            connections,
            totalCount,
            pageNumber,
            pageSize,
            (int)Math.Ceiling((double)totalCount / pageSize)
        );
    }

    public async Task DeleteConnection(int connectionId)
    {
        var connection =
            await locatorDb
                .Set<ConnectionEntity>()
                .FirstOrDefaultAsync(c => c.ConnectionId == connectionId)
            ?? throw new KeyNotFoundException($"Connection with ID {connectionId} not found.");

        locatorDb.Set<ConnectionEntity>().Remove(connection);
        await locatorDb.SaveChangesAsync();
    }
}
