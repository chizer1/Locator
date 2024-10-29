using System.Data;
using System.Data.SqlClient;
using Locator.Models.Read;
using Locator.Models.Write;
using Locator.Repositories;
using Locator.Services;

namespace Locator;

public class LocatorLib()
{
    readonly Auth0Service _auth0Service;
    readonly ClientRepository _clientRepository;
    readonly ClientUserRepository _clientUserRepository;
    readonly UserRepository _userRepository;
    readonly UserService _userService;
    readonly UserRoleRepository _userRoleRepository;
    readonly DatabaseRepository _databaseRepository;
    readonly DatabaseServerRepository _databaseServerRepository;
    readonly DatabaseTypeRepository _databaseTypeRepository;
    readonly RoleRepository _roleRepository;
    readonly RoleService _roleService;
    readonly ConnectionRepository _connectionRepository;

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
        _userService = new(_userRepository, _auth0Service);
        _userRoleRepository = new(locatorDb);
        _roleRepository = new(locatorDb);
        _roleService = new(_roleRepository, _userRoleRepository, _auth0Service);
        _clientRepository = new(locatorDb);
        _clientUserRepository = new(locatorDb);
        _connectionRepository = new(locatorDb);
        _databaseRepository = new(locatorDb);
        _databaseServerRepository = new(locatorDb);
        _databaseTypeRepository = new(locatorDb);
    }

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

    public async Task<User> GetUser(string auth0Id)
    {
        return await _userService.GetUser(auth0Id);
    }

    public async Task<User> GetUser(int userId)
    {
        return await _userService.GetUser(userId);
    }

    public async Task<List<User>> GetUsers()
    {
        return await _userService.GetUsers();
    }

    public async Task<PagedList<User>> GetUsers(string searchText, int page, int pageSize)
    {
        return await _userService.GetUsers(searchText, page, pageSize);
    }

    public async Task<List<UserLog>> GetUserLogs(string auth0Id)
    {
        return await _userService.GetUserLogs(auth0Id);
    }

    public async Task UpdateUser(
        int userId,
        string auth0Id,
        string firstName,
        string lastName,
        string emailAddress,
        UserStatus userStatus,
        List<Role> roles
    )
    {
        await _userService.UpdateUser(
            userId,
            auth0Id,
            firstName,
            lastName,
            emailAddress,
            userStatus,
            roles
        );
    }

    public async Task<int> AddRole(string name, string description)
    {
        return await _roleService.AddRole(name, description);
    }

    public async Task<List<Role>> GetRoles()
    {
        return await _roleService.GetRoles();
    }

    public async Task<Role> GetRole(int roleId)
    {
        return await _roleService.GetRole(roleId);
    }

    public async Task<Connection> GetConnection(int connectionId)
    {
        return await _connectionRepository.GetConnection(connectionId);
    }

    public async Task<SqlConnection> GetConnection(string auth0Id, int clientId, int databaseTypeId)
    {
        return await _connectionRepository.GetConnection(auth0Id, clientId, databaseTypeId);
    }

    public async Task<List<Connection>> GetConnections()
    {
        return await _connectionRepository.GetConnections();
    }

    public async Task<int> AddUserRole(User user, Role role)
    {
        return await _roleService.AddUserRole(user, role);
    }

    public async Task DeleteUserRole(User user, Role role)
    {
        await _roleService.DeleteUserRole(user, role);
    }

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
        return await _clientRepository.GetClients();
    }

    public async Task<Client> GetClient(int clientId)
    {
        return await _clientRepository.GetClient(clientId);
    }

    public async Task<int> AddClientUser(int clientId, int userId)
    {
        return await _clientUserRepository.AddClientUser(clientId, userId);
    }

    public async Task<int> AddConnection(int clientId, int databaseId)
    {
        return await _connectionRepository.AddConnection(clientId, databaseId);
    }

    public async Task<int> AddDatabase(AddDatabase addDatabase)
    {
        return await _databaseRepository.AddDatabase(addDatabase);
    }

    public async Task<List<Database>> GetDatabases()
    {
        return await _databaseRepository.GetDatabases();
    }

    public async Task<Database> GetDatabase(int databaseId)
    {
        return await _databaseRepository.GetDatabase(databaseId);
    }

    public async Task<int> AddDatabaseServer(string serverName, string serverIpAddress)
    {
        return await _databaseServerRepository.AddDatabaseServer(serverName, serverIpAddress);
    }

    public async Task<List<DatabaseServer>> GetDatabaseServers()
    {
        return await _databaseServerRepository.GetDatabaseServers();
    }

    public async Task<DatabaseServer> GetDatabaseServer(int databaseServerId)
    {
        return await _databaseServerRepository.GetDatabaseServer(databaseServerId);
    }

    public async Task<int> AddDatabaseType(string name)
    {
        return await _databaseTypeRepository.AddDatabaseType(name);
    }

    public async Task<List<DatabaseType>> GetDatabaseTypes()
    {
        return await _databaseTypeRepository.GetDatabaseTypes();
    }

    public async Task<DatabaseType> GetDatabaseType(int databaseTypeId)
    {
        return await _databaseTypeRepository.GetDatabaseType(databaseTypeId);
    }

    public async Task UpdateDatabaseType(int databaseTypeId, string name)
    {
        await _databaseTypeRepository.UpdateDatabaseType(databaseTypeId, name);
    }

    public async Task DeleteDatabaseType(int databaseTypeId)
    {
        await _databaseTypeRepository.DeleteDatabaseType(databaseTypeId);
    }

    public async Task UpdateClient(
        int clientId,
        string clientName,
        string clientCode,
        ClientStatus clientStatus
    )
    {
        await _clientRepository.UpdateClient(clientId, clientName, clientCode, clientStatus);
    }

    public async Task UpdateDatabaseServer(
        int databaseServerId,
        string serverName,
        string serverIpAddress
    )
    {
        await _databaseServerRepository.UpdateDatabaseServer(
            databaseServerId,
            serverName,
            serverIpAddress
        );
    }

    public async Task UpdateDatabase(UpdateDatabase updateDatabase)
    {
        await _databaseRepository.UpdateDatabase(updateDatabase);
    }

    public async Task DeleteDatabase(int databaseId)
    {
        await _databaseRepository.DeleteDatabase(databaseId);
    }

    public async Task UpdateRole(int roleId, string name, string description)
    {
        await _roleRepository.UpdateRole(roleId, name, description);
    }

    public async Task DeleteRole(int roleId)
    {
        await _roleRepository.DeleteRole(roleId);
    }

    public async Task DeleteClient(int clientId)
    {
        await _clientRepository.DeleteClient(clientId);
    }

    public async Task DeleteConnection(int connectionId)
    {
        await _connectionRepository.DeleteConnection(connectionId);
    }

    public async Task DeleteDatabaseServer(int databaseServerId)
    {
        await _databaseServerRepository.DeleteDatabaseServer(databaseServerId);
    }

    public async Task DeleteUser(string auth0Id)
    {
        await _userService.DeleteUser(auth0Id);
    }
}
