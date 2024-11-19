using System.Data.SqlClient;
using Locator.Common;
using Locator.Common.Models;
using Locator.Domain;
using Locator.Library;

namespace Locator;

public class Locator
{
    private readonly Clients _clients;
    private readonly ClientUsers _clientUsers;
    private readonly Connections _connections;
    private readonly Databases _databases;
    private readonly DatabaseServers _databaseServers;
    private readonly DatabaseTypes _databaseTypes;
    private readonly Users _users;

    public Locator(
        string locatorDbConnectionString,
        string auth0Url,
        string auth0ClientId,
        string auth0ClientSecret,
        string apiId,
        string apiIdentifier
    )
    {
        var locatorDb = new SqlConnection(locatorDbConnectionString);
        var auth0 = new Auth0(auth0Url, auth0ClientId, auth0ClientSecret);

        _clients = new Clients(locatorDb);
        _clientUsers = new ClientUsers(locatorDb);
        _connections = new Connections(locatorDb);
        _databases = new Databases(locatorDb);
        _databaseServers = new DatabaseServers(locatorDb);
        _databaseTypes = new DatabaseTypes(locatorDb);

        _users = new Users(locatorDb, auth0);
    }

    /// <summary>
    /// Add user to locator database and Auth0 tenant
    /// </summary>
    public async Task<int> AddUser(
        string firstName,
        string lastName,
        string emailAddress,
        string password,
        UserStatus userStatus
    )
    {
        return await _users.AddUser(firstName, lastName, emailAddress, password, userStatus);
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
        UserStatus userStatus
    )
    {
        await _users.UpdateUser(userId, firstName, lastName, emailAddress, password, userStatus);
    }

    /// <summary>
    /// Delete user from locator database and Auth0 tenant
    /// </summary>
    public async Task DeleteUser(int userId)
    {
        await _users.DeleteUser(userId);
    }
}
