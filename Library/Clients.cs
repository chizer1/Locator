using Locator.Common.Models;
using Locator.Domain;
using Locator.Features.Clients;
using Locator.Features.Clients.AddClient;
using Locator.Features.Clients.DeleteClient;
using Locator.Features.Clients.GetClients;
using Locator.Features.Clients.UpdateClient;
using Microsoft.EntityFrameworkCore;

namespace Locator.Library;

internal class Clients
{
    private readonly AddClient _addClient;
    private readonly GetClients _getClients;
    private readonly UpdateClient _updateClient;
    private readonly DeleteClient _deleteClient;

    public Clients(DbContext locatorDb)
    {
        IClientRepository clientRepository = new ClientRepository(locatorDb);

        _addClient = new AddClient(clientRepository);
        _getClients = new GetClients(clientRepository);
        _updateClient = new UpdateClient(clientRepository);
        _deleteClient = new DeleteClient(clientRepository);
    }

    public async Task<int> AddClient(string clientName, string clientCode, Status clientStatus)
    {
        return await _addClient.Handle(new AddClientCommand(clientName, clientCode, clientStatus));
    }

    public async Task<PagedList<Client>> GetClients(string keyword, int pageNumber, int pageSize)
    {
        return await _getClients.Handle(new GetClientsQuery(keyword, pageNumber, pageSize));
    }

    public async Task UpdateClient(
        int clientId,
        string clientName,
        string clientCode,
        Status clientStatus
    )
    {
        await _updateClient.Handle(
            new UpdateClientCommand(clientId, clientName, clientCode, clientStatus)
        );
    }

    public async Task DeleteClient(int clientId)
    {
        await _deleteClient.Handle(new DeleteClientCommand(clientId));
    }
}
