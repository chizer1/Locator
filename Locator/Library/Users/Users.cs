using System.Data.SqlClient;
using Locator.Common;
using Locator.Domain;
using Locator.Features.Users;

namespace Locator.Library.Users;

public class Users
{
    private readonly AddUser _addUser;

    public Users(
        SqlConnection locatorDb,
        string auth0Domain,
        string auth0ClientId,
        string auth0ClientSecret
    )
    {
        var auth0 = new Auth0(auth0Domain, auth0ClientId, auth0ClientSecret);
        _addUser = new AddUser(locatorDb, auth0Domain, auth0);
    }

    public async Task<int> AddUser(
        string firstName,
        string lastName,
        string email,
        string password,
        UserStatus status = UserStatus.Active
    )
    {
        return await _addUser.Handle(
            new AddUserCommand(firstName, lastName, email, password, status)
        );
    }
}
