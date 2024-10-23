using System.Data;
using Locator.Models;
using Locator.Repositories;
using Locator.Services;

namespace Locator;

public class LocatorX()
{
    readonly ClientRepository _clientRepository;
    readonly ClientService _clientService;

    // will also need to init auth0 credentials here
    public LocatorX(IDbConnection locatorDb)
        : this()
    {
        _clientRepository = new(locatorDb);
        _clientService = new(_clientRepository);
    }

    public async Task<int> AddClient(string clientName, string clientCode, int createById)
    {
        return await _clientService.AddClient(clientName, clientCode, createById);
    }

    public async Task<List<Client>> GetClients()
    {
        return await _clientService.GetClients();
    }

    public async Task<Client> GetClient(int clientId)
    {
        return await _clientService.GetClient(clientId);
    }

    public async Task<PagedList<Client>> GetClients(string searchText, int page, int pageSize)
    {
        return await _clientService.GetClients(searchText, page, pageSize);
    }

    public async Task UpdateClient(
        int clientId,
        string clientName,
        string clientCode,
        int clientStatusId,
        int modifyById
    )
    {
        await _clientService.UpdateClient(clientId, clientName, clientCode, clientStatusId, modifyById);
    }

    public async Task DeleteClient(int clientId)
    {
        await _clientService.DeleteClient(clientId);
    }
}
