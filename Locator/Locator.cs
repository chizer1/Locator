using System.Data.SqlClient;
using Locator.Models.Read;
using Locator.Models.Write;
using Locator.Repositories;
using Locator.Services;
using Locator.Utilities;
using static Locator.Utilities.HttpContextUtilities;

namespace Locator;

public class LocatorLib()
{
    private readonly ClientRepository _clientRepository;
    private readonly ClientUserRepository _clientUserRepository;
    private readonly UserService _userService;
    private readonly DatabaseRepository _databaseRepository;
    private readonly DatabaseServerRepository _databaseServerRepository;
    private readonly DatabaseTypeRepository _databaseTypeRepository;
    private readonly RoleService _roleService;
    private readonly ConnectionRepository _connectionRepository;
    private readonly PermissionService _permissionService;

    public LocatorLib(
        string locatorDbConnectionString,
        string auth0Url,
        string auth0ClientId,
        string auth0ClientSecret,
        string apiId,
        string apiIdentifier
    )
        : this()
    {
        var locatorDb = new SqlConnection(locatorDbConnectionString);

        Auth0Service auth0Service =
            new(auth0Url, auth0ClientId, auth0ClientSecret, apiId, apiIdentifier);
        RoleRepository roleRepository = new(locatorDb);
        UserRepository userRepository = new(locatorDb);
        _userService = new UserService(userRepository, _roleService, auth0Service);
        UserRoleRepository userRoleRepository = new(locatorDb);
        _clientRepository = new ClientRepository(locatorDb);
        _clientUserRepository = new ClientUserRepository(locatorDb);
        _connectionRepository = new ConnectionRepository(locatorDb);
        _databaseRepository = new DatabaseRepository(locatorDb);
        _databaseServerRepository = new DatabaseServerRepository(locatorDb);
        _databaseTypeRepository = new DatabaseTypeRepository(locatorDb);
        PermissionRepository permissionRepository = new(locatorDb);
        RolePermissionRepository rolePermissionRepository = new(locatorDb);
        _permissionService = new PermissionService(
            permissionRepository,
            rolePermissionRepository,
            roleRepository,
            auth0Service
        );
        _roleService = new RoleService(
            roleRepository,
            _userService,
            userRoleRepository,
            auth0Service
        );
    }

    #region User

    /// <summary>
    /// Add user to locator database and Auth0 tenant
    /// </summary>
    public async Task<int> AddUser(AddUser addUser)
    {
        return await _userService.AddUser(addUser);
    }

    /// <summary>
    /// Get user information from locator database
    /// </summary>
    public async Task<User> GetUser(string auth0Id)
    {
        return await _userService.GetUser(auth0Id);
    }

    /// <summary>
    /// Get user information from locator database
    /// </summary>
    public async Task<User> GetUser(int userId)
    {
        return await _userService.GetUser(userId);
    }

    /// <summary>
    /// Get users from locator database
    /// </summary>
    public async Task<List<User>> GetUsers()
    {
        return await _userService.GetUsers();
    }

    /// <summary>
    /// Get users from locator database with search text and pagination
    /// </summary>
    public async Task<PagedList<User>> GetUsers(string searchText, int page, int pageSize)
    {
        return await _userService.GetUsers(searchText, page, pageSize);
    }

    /// <summary>
    /// Get recent user activity from Auth0
    /// </summary>
    public async Task<List<UserLog>> GetUserLogs(string auth0Id)
    {
        return await _userService.GetUserLogs(auth0Id);
    }

    /// <summary>
    /// Get recent user activity from Auth0
    /// </summary>
    public async Task<List<UserLog>> GetUserLogs(int userId)
    {
        return await _userService.GetUserLogs(userId);
    }

    /// <summary>
    /// Update user information in locator database and Auth0 tenant
    /// </summary>
    public async Task UpdateUser(UpdateUser updateUser)
    {
        await _userService.UpdateUser(updateUser);
    }

    /// <summary>
    /// Delete user from locator database and Auth0 tenant
    /// </summary>
    public async Task DeleteUser(string auth0Id)
    {
        await _userService.DeleteUser(auth0Id);
    }

    /// <summary>
    /// Delete user from locator database and Auth0 tenant
    /// </summary>
    public async Task DeleteUser(int userId)
    {
        await _userService.DeleteUser(userId);
    }

    #endregion

    #region Role

    /// <summary>
    /// Add role to locator database and Auth0 tenant
    /// </summary>
    public async Task<int> AddRole(AddRole addRole)
    {
        return await _roleService.AddRole(addRole);
    }

    /// <summary>
    /// Get roles from locator database
    /// </summary>
    public async Task<List<Role>> GetRoles()
    {
        return await _roleService.GetRoles();
    }

    /// <summary>
    /// Get role information from locator database
    /// </summary>
    public async Task<Role> GetRole(int roleId)
    {
        return await _roleService.GetRole(roleId);
    }

    /// <summary>
    /// Update role information in locator database and Auth0 tenant
    /// </summary>
    public async Task UpdateRole(UpdateRole updateRole)
    {
        await _roleService.UpdateRole(updateRole);
    }

    /// <summary>
    /// Delete role from locator database and Auth0 tenant
    /// </summary>
    public async Task DeleteRole(int roleId)
    {
        await _roleService.DeleteRole(roleId);
    }

    #endregion

    #region Connection

    /// <summary>
    /// Get connection from locator database
    /// </summary>
    public async Task<Connection> GetConnection(int connectionId)
    {
        return await _connectionRepository.GetConnection(connectionId);
    }

    /// <summary>
    /// Create SQL connection based on user, client and database type
    /// </summary>
    public async Task<SqlConnection> GetConnection(string auth0Id, int clientId, int databaseTypeId)
    {
        return await _connectionRepository.GetConnection(auth0Id, clientId, databaseTypeId);
    }

    /// <summary>
    ///Get connections from locator database
    /// </summary>
    public async Task<List<Connection>> GetConnections()
    {
        return await _connectionRepository.GetConnections();
    }

    /// <summary>
    ///Add connection to locator database
    /// </summary>
    public async Task<int> AddConnection(int clientUserId, int databaseId)
    {
        return await _connectionRepository.AddConnection(clientUserId, databaseId);
    }

    /// <summary>
    ///Delete connection from locator database
    /// </summary>
    public async Task DeleteConnection(int clientId, int userId)
    {
        await _connectionRepository.DeleteConnection(clientId, userId);
    }

    #endregion

    #region User Role

    /// <summary>
    ///Add user to a role in locator database and Auth0 tenant
    /// </summary>
    public async Task<int> AddUserRole(int userId, int roleId)
    {
        return await _roleService.AddUserRole(userId, roleId);
    }

    /// <summary>
    ///Remove user from a role in locator database and Auth0 tenant
    /// </summary>
    public async Task DeleteUserRole(int userId, int roleId)
    {
        await _roleService.DeleteUserRole(userId, roleId);
    }

    #endregion

    #region Client

    /// <summary>
    ///Add client to locator database
    /// </summary>
    public async Task<int> AddClient(AddClient addClient)
    {
        return await _clientRepository.AddClient(addClient);
    }

    /// <summary>
    ///Gets clients from locator database
    /// </summary>
    public async Task<List<Client>> GetClients()
    {
        return await _clientRepository.GetClients();
    }

    /// <summary>
    ///Get client information from locator database
    /// </summary>
    public async Task<Client> GetClient(int clientId)
    {
        return await _clientRepository.GetClient(clientId);
    }

    /// <summary>
    ///Update client information in locator database
    /// </summary>
    public async Task UpdateClient(UpdateClient updateClient)
    {
        await _clientRepository.UpdateClient(updateClient);
    }

    /// <summary>
    ///Delete client from locator database
    /// </summary>
    public async Task DeleteClient(int clientId)
    {
        await _clientRepository.DeleteClient(clientId);
    }

    #endregion

    #region ClientUser

    /// <summary>
    ///Add user to a client in locator database
    /// </summary>
    public async Task<int> AddClientUser(int clientId, int userId)
    {
        return await _clientUserRepository.AddClientUser(clientId, userId);
    }

    /// <summary>
    ///Remove user from a client in locator database
    /// </summary>
    public async Task DeleteClientUser(int clientId, int userId)
    {
        await _clientUserRepository.DeleteClientUser(clientId, userId);
    }

    #endregion

    #region Database

    /// <summary>
    ///Add new database on specified server and insert record in locator database
    /// </summary>
    public async Task<int> AddDatabase(AddDatabase addDatabase)
    {
        return await _databaseRepository.AddDatabase(addDatabase);
    }

    /// <summary>
    ///Get databases from locator database
    /// </summary>
    public async Task<List<Database>> GetDatabases()
    {
        return await _databaseRepository.GetDatabases();
    }

    /// <summary>
    ///Get database information from locator database
    /// </summary>
    public async Task<Database> GetDatabase(int databaseId)
    {
        return await _databaseRepository.GetDatabase(databaseId);
    }

    /// <summary>
    ///Update database information in locator database and make updates on database server
    /// </summary>
    public async Task UpdateDatabase(UpdateDatabase updateDatabase)
    {
        await _databaseRepository.UpdateDatabase(updateDatabase);
    }

    /// <summary>
    ///Delete database information from locator database and on database server
    /// </summary>
    public async Task DeleteDatabase(int databaseId)
    {
        await _databaseRepository.DeleteDatabase(databaseId);
    }

    #endregion

    #region Database Server

    public async Task<int> AddDatabaseServer(AddDatabaseServer addDatabaseServer)
    {
        return await _databaseServerRepository.AddDatabaseServer(addDatabaseServer);
    }

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

    public async Task<int> AddRolePermission(int roleId, int permissionId)
    {
        return await _permissionService.AddRolePermission(roleId, permissionId);
    }

    #endregion

    public static string GetAuth0Id(HttpContext httpContext)
    {
        return HttpContextUtilities.GetAuth0Id(httpContext);
    }
}
