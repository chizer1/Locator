using System.Data;
using Locator.Models;
using Locator.Repositories;
using Locator.Services;

namespace Locator;

public class LocatorLib()
{
    readonly ClientRepository _clientRepository;
    readonly UserRepository _userRepository;
    readonly DatabaseRepository _databaseRepository;
    readonly RoleRepository _roleRepository;
    readonly ClientService _clientService;
    readonly UserService _userService;
    readonly DatabaseService _databaseService;
    readonly Auth0Service _auth0Service;

    public LocatorLib(
        IDbConnection locatorDb,
        string auth0Url,
        string auth0ClientId,
        string auth0ClientSecret
    )
        : this()
    {
        _auth0Service = new(auth0Url, auth0ClientId, auth0ClientSecret);
        _clientRepository = new(locatorDb);
        _userRepository = new(locatorDb);
        _roleRepository = new(locatorDb);
        _clientService = new(_clientRepository);

        _userService = new(_userRepository, new RoleRepository(locatorDb), _auth0Service);
        _databaseRepository = new(locatorDb);
        _databaseService = new(_databaseRepository);
    }

    #region Client

    public async Task<int> AddClient(
        string clientName,
        string clientCode,
        ClientStatus clientStatus
    )
    {
        return await _clientRepository.AddClient(clientName, clientCode, clientStatus);
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
        ClientStatus clientStatus
    )
    {
        await _clientService.UpdateClient(clientId, clientName, clientCode, clientStatus);
    }

    public async Task DeleteClient(int clientId)
    {
        await _clientService.DeleteClient(clientId);
    }

    #endregion

    #region User

    public async Task<int> AddUser(
        string firstName,
        string lastName,
        string emailAddress,
        int[] roleIds,
        UserStatus userStatus,
        int clientId
    )
    {
        return await _userService.AddUser(
            firstName,
            lastName,
            emailAddress,
            roleIds,
            userStatus,
            clientId
        );
    }

    // get users
    public async Task<List<User>> GetUsers()
    {
        return await _userService.GetUsers();
    }

    #endregion

    #region Database

    // add database method

    public Task<int> AddDatabaseServer(string databaseServerName, string databaseServerIpAddress)
    {
        return _databaseService.AddDatabaseServer(databaseServerName, databaseServerIpAddress);
    }

    public Task<int> AddDatabaseType(string databaseTypeName)
    {
        return _databaseService.AddDatabaseType(databaseTypeName);
    }

    public Task<int> AddDatabase(
        string databaseName,
        string databaseUser,
        string databaseUserPassword,
        int databaseServerId,
        int databaseTypeId,
        DatabaseStatus databaseStatus
    )
    {
        return _databaseService.AddDatabase(
            databaseName,
            databaseUser,
            databaseUserPassword,
            databaseServerId,
            databaseTypeId,
            databaseStatus
        );
    }

    public Task<List<DatabaseServer>> GetDatabaseServers()
    {
        return _databaseService.GetDatabaseServers();
    }

    public Task<List<DatabaseType>> GetDatabaseTypes()
    {
        return _databaseService.GetDatabaseTypes();
    }

    public Task<List<Database>> GetDatabases()
    {
        return _databaseService.GetDatabases();
    }

    #endregion
}
