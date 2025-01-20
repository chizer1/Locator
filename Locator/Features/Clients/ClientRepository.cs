using Locator.Common.Models;
using Locator.Db;
using Locator.Domain;
using Microsoft.EntityFrameworkCore;

namespace Locator.Features.Clients;

internal class ClientRepository(DbContext locatorDb) : IClientRepository
{
    public async Task<int> AddClient(string clientName, string clientCode, Status clientStatus)
    {
        var client = new ClientEntity
        {
            ClientName = clientName,
            ClientCode = clientCode,
            ClientStatusId = (int)clientStatus,
        };

        await locatorDb.Set<ClientEntity>().AddAsync(client);
        await locatorDb.SaveChangesAsync();

        return client.ClientId;
    }

    public async Task<Client> GetClient(int clientId)
    {
        var clientEntity =
            await locatorDb.Set<ClientEntity>().FirstOrDefaultAsync(c => c.ClientId == clientId)
            ?? throw new KeyNotFoundException($"Client with ID {clientId} not found.");

        return new Client(
            clientEntity.ClientId,
            clientEntity.ClientName,
            clientEntity.ClientCode,
            (Status)clientEntity.ClientStatusId
        );
    }

    public async Task<PagedList<Client>> GetClients(string keyword, int pageNumber, int pageSize)
    {
        var query = locatorDb.Set<ClientEntity>().AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(c => EF.Functions.Like(c.ClientName, $"%{keyword}%"));

        var totalCount = await query.CountAsync();

        var clientEntities = await query
            .OrderBy(c => c.ClientName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var clients = clientEntities.Select(entity => new Client(
            entity.ClientId,
            entity.ClientName,
            entity.ClientCode,
            (Status)entity.ClientStatusId
        ));

        return new PagedList<Client>(
            clients,
            totalCount,
            pageNumber,
            pageSize,
            (int)Math.Ceiling((double)totalCount / pageSize)
        );
    }

    public async Task UpdateClient(
        int clientId,
        string clientName,
        string clientCode,
        Status clientStatus
    )
    {
        var client =
            await locatorDb.Set<ClientEntity>().FirstOrDefaultAsync(c => c.ClientId == clientId)
            ?? throw new KeyNotFoundException($"Client with ID {clientId} not found.");

        client.ClientName = clientName;
        client.ClientCode = clientCode;
        client.ClientStatusId = (int)clientStatus;

        locatorDb.Set<ClientEntity>().Update(client);
        await locatorDb.SaveChangesAsync();
    }

    public async Task DeleteClient(int clientId)
    {
        var client =
            await locatorDb.Set<ClientEntity>().FirstOrDefaultAsync(c => c.ClientId == clientId)
            ?? throw new KeyNotFoundException($"Client with ID {clientId} not found.");

        locatorDb.Set<ClientEntity>().Remove(client);
        await locatorDb.SaveChangesAsync();
    }
}
