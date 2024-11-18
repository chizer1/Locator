using System.Data.SqlClient;
using Locator.Common.Models;
using Locator.Domain;
using Locator.Features.Clients;
using Locator.Features.Clients.AddClient;
using Locator.Features.Clients.DeleteClient;
using Locator.Features.Clients.GetClients;
using Locator.Features.Clients.UpdateClient;

namespace Locator.Library;

public class Clients
{
    private readonly AddClient _addClient;
    private readonly GetClients _getClients;
    private readonly UpdateClient _updateClient;
    private readonly DeleteClient _deleteClient;

    public Clients(SqlConnection locatorDb)
    {
        IClientRepository clientRepository = new ClientRepository(locatorDb);

        _addClient = new AddClient(clientRepository);
        _getClients = new GetClients(clientRepository);
        _updateClient = new UpdateClient(clientRepository);
        _deleteClient = new DeleteClient(clientRepository);
    }

    public async Task<int> AddClient(
        string clientName,
        string clientCode,
        ClientStatus clientStatus
    )
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
        ClientStatus status
    )
    {
        await _updateClient.Handle(
            new UpdateClientCommand(clientId, clientName, clientCode, status)
        );
    }

    public async Task DeleteClient(int clientId)
    {
        await _deleteClient.Handle(new DeleteClientCommand(clientId));
    }
}
