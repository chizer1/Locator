using System.Data;
using System.Data.SqlClient;
using Locator.Models;
using Locator.Repositories;
using Locator.Services;

namespace Locator;

public class LocatorLib()
{
    readonly Auth0Service _auth0Service;
    readonly ClientRepository _clientRepository;
    readonly ClientService _clientService;
    readonly UserRepository _userRepository;
    readonly UserService _userService;
    readonly DatabaseRepository _databaseRepository;
    readonly DatabaseService _databaseService;
    readonly RoleRepository _roleRepository;
    readonly RoleService _roleService;
    readonly ConnectionRepository _connectionRepository;
    readonly ConnectionService _connectionService;

    public LocatorLib(
        IDbConnection locatorDb,
        string auth0Url,
        string auth0ClientId,
        string auth0ClientSecret
    )
        : this()
    {
        _auth0Service = new(auth0Url, auth0ClientId, auth0ClientSecret);

        _userRepository = new(locatorDb);
        _userService = new(_userRepository, new RoleRepository(locatorDb), _auth0Service);
        _roleRepository = new(locatorDb);
        _roleService = new(_roleRepository);
        _clientRepository = new(locatorDb);
        _clientService = new(_clientRepository);
        _connectionRepository = new(locatorDb);
        _connectionService = new(_connectionRepository);
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
        List<Role> roles,
        UserStatus userStatus
    )
    {
        return await _userService.AddUser(firstName, lastName, emailAddress, roles, userStatus);
    }

    // get users
    public async Task<List<User>> GetUsers()
    {
        return await _userService.GetUsers();
    }

    #endregion

    #region ClientUser

    // add client user
    public async Task<int> AddClientUser(int clientId, int userId)
    {
        return await _clientRepository.AddClientUser(clientId, userId);
    }

    #endregion

    #region Database


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

    #region Connection

    public Task<int> AddConnection(int databaseId, int userId)
    {
        return _connectionService.AddConnection(databaseId, userId);
    }

    public Task<Connection> GetConnection(int connectionId)
    {
        return _connectionService.GetConnection(connectionId);
    }

    public Task<SqlConnection> GetConnection(string auth0Id, int clientId, int databaseTypeId)
    {
        return _connectionService.GetConnection(auth0Id, clientId, databaseTypeId);
    }

    #endregion

    #region Role

    // add role
    public async Task<int> AddRole(string roleName, string roleDescription)
    {
        return await _roleService.AddRole(roleName, roleDescription);
    }

    // add user role
    public async Task<int> AddUserRole(int userId, int roleId)
    {
        return await _roleRepository.AddUserRole(userId, roleId);
    }

    #endregion

    // add client database
    public async Task<int> AddClientDatabase(int clientId, int databaseId)
    {
        return await _clientRepository.AddClientDatabase(clientId, databaseId);
    }
}
