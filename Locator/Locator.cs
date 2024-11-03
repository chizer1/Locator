using System.Data;
using System.Data.SqlClient;
using Locator.Models.Read;
using Locator.Models.Write;
using Locator.Repositories;
using Locator.Services;
using Locator.Utilities;

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
    readonly PermissionRepository _permissionRepository;
    readonly PermissionService _permissionService;

    public LocatorLib(
        IDbConnection locatorDb,
        string auth0Url,
        string auth0ClientId,
        string auth0ClientSecret
    )
        : this()
    {
        _auth0Service = new(auth0Url, auth0ClientId, auth0ClientSecret);
        _roleRepository = new(locatorDb);
        _roleService = new(_roleRepository, _userService, _userRoleRepository, _auth0Service);
        _userRepository = new(locatorDb);
        _userService = new(_userRepository, _roleService, _auth0Service);
        _userRoleRepository = new(locatorDb);
        _clientRepository = new(locatorDb);
        _clientUserRepository = new(locatorDb);
        _connectionRepository = new(locatorDb);
        _databaseRepository = new(locatorDb);
        _databaseServerRepository = new(locatorDb);
        _databaseTypeRepository = new(locatorDb);
        _permissionRepository = new(locatorDb);
        _permissionService = new(_permissionRepository, _auth0Service);
    }

    #region User

    public async Task<int> AddUser(AddUser addUser)
    {
        return await _userService.AddUser(addUser);
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

    public async Task UpdateUser(UpdateUser updateUser)
    {
        await _userService.UpdateUser(updateUser);
    }

    public async Task DeleteUser(string auth0Id)
    {
        await _userService.DeleteUser(auth0Id);
    }

    #endregion

    #region Role

    public async Task<int> AddRole(AddRole addRole)
    {
        return await _roleService.AddRole(addRole);
    }

    public async Task<List<Role>> GetRoles()
    {
        return await _roleService.GetRoles();
    }

    public async Task<Role> GetRole(int roleId)
    {
        return await _roleService.GetRole(roleId);
    }

    public async Task UpdateRole(UpdateRole updateRole)
    {
        await _roleService.UpdateRole(updateRole);
    }

    public async Task DeleteRole(int roleId)
    {
        await _roleService.DeleteRole(roleId);
    }

    #endregion

    #region Connection

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

    public async Task<int> AddConnection(int clientUserId, int databaseId)
    {
        return await _connectionRepository.AddConnection(clientUserId, databaseId);
    }

    public async Task DeleteConnection(int clientId, int userId)
    {
        await _connectionRepository.DeleteConnection(clientId, userId);
    }

    #endregion

    #region User Role

    public async Task<int> AddUserRole(int userId, int roleId)
    {
        return await _roleService.AddUserRole(userId, roleId);
    }

    public async Task DeleteUserRole(int userId, int roleId)
    {
        await _roleService.DeleteUserRole(userId, roleId);
    }

    #endregion

    #region Client

    public async Task<int> AddClient(AddClient addClient)
    {
        return await _clientRepository.AddClient(addClient);
    }

    public async Task<List<Client>> GetClients()
    {
        return await _clientRepository.GetClients();
    }

    public async Task<Client> GetClient(int clientId)
    {
        return await _clientRepository.GetClient(clientId);
    }

    public async Task UpdateClient(UpdateClient updateClient)
    {
        await _clientRepository.UpdateClient(updateClient);
    }

    public async Task DeleteClient(int clientId)
    {
        await _clientRepository.DeleteClient(clientId);
    }

    #endregion

    #region ClientUser

    public async Task<int> AddClientUser(int clientId, int userId)
    {
        return await _clientUserRepository.AddClientUser(clientId, userId);
    }

    public async Task DeleteClientUser(int clientId, int userId)
    {
        await _clientUserRepository.DeleteClientUser(clientId, userId);
    }

    #endregion

    #region Database

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

    public async Task UpdateDatabase(UpdateDatabase updateDatabase)
    {
        await _databaseRepository.UpdateDatabase(updateDatabase);
    }

    public async Task DeleteDatabase(int databaseId)
    {
        await _databaseRepository.DeleteDatabase(databaseId);
    }

    #endregion

    #region Database Server

    public async Task<List<DatabaseServer>> GetDatabaseServers()
    {
        return await _databaseServerRepository.GetDatabaseServers();
    }

    public async Task<DatabaseServer> GetDatabaseServer(int databaseServerId)
    {
        return await _databaseServerRepository.GetDatabaseServer(databaseServerId);
    }

    public async Task UpdateDatabaseServer(UpdateDatabaseServer updateDatabaseServer)
    {
        await _databaseServerRepository.UpdateDatabaseServer(updateDatabaseServer);
    }

    public async Task<int> AddDatabaseServer(AddDatabaseServer addDatabaseServer)
    {
        return await _databaseServerRepository.AddDatabaseServer(addDatabaseServer);
    }

    public async Task DeleteDatabaseServer(int databaseServerId)
    {
        await _databaseServerRepository.DeleteDatabaseServer(databaseServerId);
    }

    #endregion

    #region Database Type

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

    #endregion

    #region Permission

    public async Task AddPermission(string permissionName, string permissionDescription)
    {
        await _permissionService.AddPermission(permissionName, permissionDescription);
    }

    #endregion
    public string GetAuth0Id(HttpContext httpContext)
    {
        return HttpContextUtilities.GetAuth0Id(httpContext);
    }
}
