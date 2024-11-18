using System.Data.SqlClient;
using Locator.Features.ClientUsers;
using Locator.Features.ClientUsers.AddClientUser;
using Locator.Features.ClientUsers.DeleteClientUser;
using AddClientUser = Locator.Features.ClientUsers.AddClientUser.AddClientUser;

namespace Locator.Library;

public class ClientUsers
{
    private readonly AddClientUser _addClientUser;
    private readonly DeleteClientUser _deleteClientUser;

    public ClientUsers(SqlConnection locatorDb)
    {
        IClientUserRepository clientRepository = new ClientUserRepository(locatorDb);

        _addClientUser = new AddClientUser(clientRepository);
        _deleteClientUser = new DeleteClientUser(clientRepository);
    }

    public async Task<int> AddClientUser(int clientId, int userId)
    {
        return await _addClientUser.Handle(new AddClientUserCommand(clientId, userId));
    }

    public async Task DeleteClient(int clientId, int userId)
    {
        await _deleteClientUser.Handle(new DeleteClientUserCommand(clientId, userId));
    }
}
