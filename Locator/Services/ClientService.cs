using Locator.Models;
using Locator.Repositories;

namespace Locator.Services;

internal class ClientService(ClientRepository clientRepository)
{
    public async Task<int> AddClient(
        string clientName,
        string clientCode,
        ClientStatus clientStatus
    )
    {
        return await clientRepository.AddClient(clientName, clientCode, clientStatus);
    }

    public async Task<Client> GetClient(int clientId)
    {
        return await clientRepository.GetClient(clientId);
    }

    public async Task<List<Client>> GetClients()
    {
        return await clientRepository.GetClients();
    }

    public async Task<PagedList<Client>> GetClients(string searchText, int page, int pageSize)
    {
        return await clientRepository.GetClients(searchText, page, pageSize);
    }

    public async Task UpdateClient(
        int clientId,
        string clientName,
        string clientCode,
        ClientStatus clientStatus
    )
    {
        await clientRepository.UpdateClient(clientId, clientName, clientCode, clientStatus);
    }

    public async Task DeleteClient(int clientId)
    {
        await clientRepository.DeleteClient(clientId);
    }

    // add client database
    public async Task<int> AddClientDatabase(
        int clientId,
        int databaseId
    )
    {
        return await clientRepository.AddClientDatabase(clientId, databaseId);
    }
}
