using System.Data.SqlClient;
using Locator.Library.Users;

namespace Locator;

public class Locator
{
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

        _users = new Users(locatorDb, auth0Url, auth0ClientId, auth0ClientSecret);
    }

    /// <summary>
    /// Add user to locator database and Auth0 tenant
    /// </summary>
    public async Task<int> AddUser(
        string firstName,
        string lastName,
        string emailAddress,
        string password
    )
    {
        return await _users.AddUser(firstName, lastName, emailAddress, password);
    }
}
