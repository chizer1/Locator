using Locator.Common.Models;
using Locator.Domain;

namespace Locator.Features.Clients;

public interface IClientRepository
{
    public Task<int> AddClient(string clientName, string clientCode, ClientStatus clientStatus);
    public Task<Client> GetClient(int clientId);
    public Task<PagedList<Client>> GetClients(string keyword, int pageNumber, int pageSize);
    public Task UpdateClient(
        int clientId,
        string clientName,
        string clientCode,
        ClientStatus clientStatus
    );
    public Task DeleteClient(int clientId);
}
