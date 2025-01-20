using Locator.Features.ClientUsers;
using Locator.Features.ClientUsers.AddClientUser;
using Locator.Features.ClientUsers.DeleteClientUser;
using Microsoft.EntityFrameworkCore;

namespace Locator.Library;

internal class ClientUsers
{
    private readonly AddClientUser _addClientUser;
    private readonly DeleteClientUser _deleteClientUser;

    public ClientUsers(DbContext locatorDb)
    {
        IClientUserRepository clientUserRepository = new ClientUserRepository(locatorDb);

        _addClientUser = new AddClientUser(clientUserRepository);
        _deleteClientUser = new DeleteClientUser(clientUserRepository);
    }

    public async Task<int> AddClientUser(int clientId, int userId)
    {
        return await _addClientUser.Handle(new AddClientUserCommand(clientId, userId));
    }

    public async Task DeleteClientUser(int clientId, int userId)
    {
        await _deleteClientUser.Handle(new DeleteClientUserCommand(clientId, userId));
    }
}
