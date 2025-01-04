using System.Data.SqlClient;
using Locator.Common;
using Locator.Common.Models;
using Locator.Db;
using Locator.Domain;
using Locator.Library;
using Locator.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Locator;

public class Locator
{
    private readonly Clients _clients;
    private readonly ClientUsers _clientUsers;
    private readonly Connections _connections;
    private readonly Databases _databases;
    private readonly DatabaseServers _databaseServers;
    private readonly DatabaseTypes _databaseTypes;
    private readonly Permissions _permissions;
    private readonly RolePermissions _rolePermissions;
    private readonly Roles _roles;
    private readonly UserRoles _userRoles;
    private readonly Users _users;
    private readonly HttpContextUtilities _httpContextUtilities;

    public Locator(
        string locatorDbConnectionString,
        string auth0Url,
        string auth0ClientId,
        string auth0ClientSecret,
        string apiId,
        string apiIdentifier
    )
    {
        var options = new DbContextOptionsBuilder<LocatorContext>()
            .UseSqlServer(locatorDbConnectionString)
            .Options;

        var locatorDb = new LocatorContext(options);
        if (!locatorDb.Database.CanConnect())
            throw new Exception("Could not connect to database");
        else
            locatorDb.Database.Migrate();

        var auth0 = new Auth0(auth0Url, auth0ClientId, auth0ClientSecret, apiId, apiIdentifier);

        _clients = new Clients(locatorDb);
        _clientUsers = new ClientUsers(locatorDb);
        _connections = new Connections(locatorDb);
        _databases = new Databases(locatorDb);
        _databaseServers = new DatabaseServers(locatorDb);
        _databaseTypes = new DatabaseTypes(locatorDb);
        _permissions = new Permissions(locatorDb, auth0);
        _rolePermissions = new RolePermissions(locatorDb, auth0);
        _roles = new Roles(locatorDb, auth0);
        _userRoles = new UserRoles(locatorDb, auth0);
        _users = new Users(locatorDb, auth0);
        _httpContextUtilities = new HttpContextUtilities();
    }

    #region Clients

    /// <summary>
    ///Add client to locator database
    /// </summary>
    public async Task<int> AddClient(string clientName, string clientCode, Status clientStatus)
    {
        return await _clients.AddClient(clientName, clientCode, clientStatus);
    }

    /// <summary>
    ///Gets clients from locator database with search text and pagination
    /// </summary>
    public async Task<PagedList<Client>> GetClients(string searchText, int page, int pageSize)
    {
        return await _clients.GetClients(searchText, page, pageSize);
    }

    /// <summary>
    ///Update client information in locator database
    /// </summary>
    public async Task UpdateClient(
        int clientId,
        string clientName,
        string clientCode,
        Status clientStatus
    )
    {
        await _clients.UpdateClient(clientId, clientName, clientCode, clientStatus);
    }

    /// <summary>
    ///Delete client from locator database
    /// </summary>
    public async Task DeleteClient(int clientId)
    {
        await _clients.DeleteClient(clientId);
    }

    #endregion

    #region ClientUsers

    /// <summary>
    ///Add user to a client in locator database
    /// </summary>
    public async Task<int> AddClientUser(int clientId, int userId)
    {
        return await _clientUsers.AddClientUser(clientId, userId);
    }

    /// <summary>
    ///Remove user from a client in locator database
    /// </summary>
    public async Task DeleteClientUser(int clientId, int userId)
    {
        await _clientUsers.DeleteClientUser(clientId, userId);
    }

    #endregion

    #region Connections

    /// <summary>
    /// Create SQL connection based on user, client and database type
    /// </summary>
    public async Task<SqlConnection> GetConnection(string auth0Id, int clientId, int databaseTypeId)
    {
        return await _connections.GetConnection(auth0Id, clientId, databaseTypeId);
    }

    /// <summary>
    ///Add connection to locator database
    /// </summary>
    public async Task<int> AddConnection(int clientUserId, int databaseId)
    {
        return await _connections.AddConnection(clientUserId, databaseId);
    }

    /// <summary>
    ///Delete connection from locator database
    /// </summary>
    public async Task DeleteConnection(int connectionId)
    {
        await _connections.DeleteConnection(connectionId);
    }

    #endregion

    #region Databases

    /// <summary>
    ///Add new database on specified server and insert record in locator database
    /// </summary>
    public async Task<int> AddDatabase(
        string databaseName,
        string databaseUser,
        int databaseServerId,
        byte databaseTypeId,
        Status databaseStatus
    )
    {
        return await _databases.AddDatabase(
            databaseName,
            databaseUser,
            databaseServerId,
            databaseTypeId,
            databaseStatus
        );
    }

    /// <summary>
    ///Get databases from locator database with search text and pagination
    /// </summary>
    public async Task<PagedList<Database>> GetDatabases(string searchText, int page, int pageSize)
    {
        return await _databases.GetDatabases(searchText, page, pageSize);
    }

    /// <summary>
    ///Update database information in locator database and make updates on database server
    /// </summary>
    public async Task UpdateDatabase(
        int databaseId,
        string databaseName,
        string databaseUser,
        int databaseServerId,
        byte databaseTypeId,
        Status databaseStatus
    )
    {
        await _databases.UpdateDatabase(
            databaseId,
            databaseName,
            databaseUser,
            databaseServerId,
            databaseTypeId,
            databaseStatus
        );
    }

    /// <summary>
    ///Delete database information from locator database and on server it lives on
    /// </summary>
    public async Task DeleteDatabase(int databaseId)
    {
        await _databases.DeleteDatabase(databaseId);
    }

    #endregion

    #region DatabaseServers

    /// <summary>
    ///Add existing database server information to locator database. (This will not create spin up a database server for you)
    /// </summary>
    public async Task<int> AddDatabaseServer(
        string databaseServerName,
        string databaseServerIpAddress
    )
    {
        return await _databaseServers.AddDatabaseServer(
            databaseServerName,
            databaseServerIpAddress
        );
    }

    /// <summary>
    ///Get database servers from locator database
    /// </summary>
    public async Task<List<DatabaseServer>> GetDatabaseServers()
    {
        return await _databaseServers.GetDatabaseServers();
    }

    /// <summary>
    ///Update database server information in locator database
    /// </summary>
    public async Task UpdateDatabaseServer(
        int databaseServerId,
        string databaseServerName,
        string databaseServerIpAddress
    )
    {
        await _databaseServers.UpdateDatabaseServer(
            databaseServerId,
            databaseServerName,
            databaseServerIpAddress
        );
    }

    /// <summary>
    ///Delete database server information from locator database
    /// </summary>
    public async Task DeleteDatabaseServer(int databaseServerId)
    {
        await _databaseServers.DeleteDatabaseServer(databaseServerId);
    }

    #endregion

    #region DatabaseTypes

    /// <summary>
    ///Add database type to locator database
    /// </summary>
    public async Task<int> AddDatabaseType(string name)
    {
        return await _databaseTypes.AddDatabaseType(name);
    }

    /// <summary>
    ///Get database types from locator database
    /// </summary>
    public async Task<List<DatabaseType>> GetDatabaseTypes()
    {
        return await _databaseTypes.GetDatabaseTypes();
    }

    /// <summary>
    ///Update database type information in locator database
    /// </summary>
    public async Task UpdateDatabaseType(int databaseTypeId, string name)
    {
        await _databaseTypes.UpdateDatabaseType(databaseTypeId, name);
    }

    /// <summary>
    ///Delete database type from locator database
    /// </summary>
    public async Task DeleteDatabaseType(int databaseTypeId)
    {
        await _databaseTypes.DeleteDatabaseType(databaseTypeId);
    }

    #endregion

    #region Permissions

    /// <summary>
    ///Add permission to locator database and to Auth0 API
    /// </summary>
    public async Task AddPermission(string permissionName, string permissionDescription)
    {
        await _permissions.AddPermission(permissionName, permissionDescription);
    }

    /// <summary>
    ///Get permission information in locator database
    /// </summary>
    public async Task<List<Permission>> GetPermissions()
    {
        return await _permissions.GetPermissions();
    }

    /// <summary>
    ///Update permission information in locator database and to Auth0 API
    /// </summary>
    public async Task UpdatePermission(
        int permissionId,
        string permissionName,
        string permissionDescription
    )
    {
        await _permissions.UpdatePermission(permissionId, permissionName, permissionDescription);
    }

    /// <summary>
    ///Delete permission in locator database and Auth0 API
    /// </summary>
    public async Task DeletePermission(int permissionId)
    {
        await _permissions.DeletePermission(permissionId);
    }

    #endregion

    #region RolePermissions

    /// <summary>
    ///Add permission to a role in locator database and Auth0 API
    /// </summary>
    public async Task<int> AddRolePermission(int roleId, int permissionId)
    {
        return await _rolePermissions.AddRolePermission(roleId, permissionId);
    }

    /// <summary>
    ///Delete permission from a role in locator database and Auth0 API
    /// </summary>
    public async Task DeleteRolePermission(int roleId, int permissionId)
    {
        await _rolePermissions.DeleteRolePermission(roleId, permissionId);
    }

    #endregion

    #region Roles

    /// <summary>
    /// Add role to locator database and Auth0 tenant
    /// </summary>
    public async Task<int> AddRole(string roleName, string roleDescription)
    {
        return await _roles.AddRole(roleName, roleDescription);
    }

    /// <summary>
    /// Get roles from locator database
    /// </summary>
    public async Task<List<Role>> GetRoles()
    {
        return await _roles.GetRoles();
    }

    /// <summary>
    /// Update role information in locator database and Auth0 tenant
    /// </summary>
    public async Task UpdateRole(int roleId, string roleName, string roleDescription)
    {
        await _roles.UpdateRole(roleId, roleName, roleDescription);
    }

    /// <summary>
    /// Delete role from locator database and Auth0 tenant
    /// </summary>
    public async Task DeleteRole(int roleId)
    {
        await _roles.DeleteRole(roleId);
    }

    #endregion

    #region UserRoles

    /// <summary>
    ///Add user to a role in locator database and Auth0 tenant
    /// </summary>
    public async Task<int> AddUserRole(int userId, int roleId)
    {
        return await _userRoles.AddUserRole(userId, roleId);
    }

    /// <summary>
    ///Remove user from a role in locator database and Auth0 tenant
    /// </summary>
    public async Task DeleteUserRole(int userId, int roleId)
    {
        await _userRoles.DeleteUserRole(userId, roleId);
    }

    #endregion

    #region Users

    /// <summary>
    /// Add user to locator database and Auth0 tenant
    /// </summary>
    public async Task<int> AddUser(
        string firstName,
        string lastName,
        string emailAddress,
        string password,
        Status userStatus
    )
    {
        return await _users.AddUser(firstName, lastName, emailAddress, password, userStatus);
    }

    /// <summary>
    /// Generate Url for user to go update their password in Auth0
    /// </summary>
    public async Task<string> GeneratePasswordChangeTicket(string auth0Id, string redirectUrl)
    {
        return await _users.GeneratePasswordChangeTicket(auth0Id, redirectUrl);
    }

    /// <summary>
    /// Get users from locator database
    /// </summary>
    public async Task<PagedList<User>> GetUsers(string keyWord, int pageNumber, int pageSize)
    {
        return await _users.GetUsers(keyWord, pageNumber, pageSize);
    }

    /// <summary>
    /// Update user information in locator database and Auth0 tenant
    /// </summary>
    public async Task UpdateUser(
        int userId,
        string firstName,
        string lastName,
        string emailAddress,
        string password,
        Status userStatus
    )
    {
        await _users.UpdateUser(userId, firstName, lastName, emailAddress, password, userStatus);
    }

    /// <summary>
    /// Update user password in Auth0
    /// </summary>
    public async Task UpdateUserPassword(string auth0, string password)
    {
        await _users.UpdateUserPassword(auth0, password);
    }

    /// <summary>
    /// Delete user from locator database and Auth0 tenant
    /// </summary>
    public async Task DeleteUser(int userId)
    {
        await _users.DeleteUser(userId);
    }

    #endregion

    /// <summary>
    ///Get a user's Auth0Id from an authenticated http request
    /// </summary>
    public string GetAuth0Id(HttpContext httpContext)
    {
        return _httpContextUtilities.GetAuth0Id(httpContext);
    }
}
