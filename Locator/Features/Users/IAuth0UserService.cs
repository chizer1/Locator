using Locator.Domain;

namespace Locator.Features.Users;

public interface IAuth0UserService
{
    public Task<string> AddUser(
        string firstName,
        string lastName,
        string emailAddress,
        string password,
        UserStatus userStatus
    );

    public Task UpdateUser(
        string auth0Id,
        string firstName,
        string lastName,
        string emailAddress,
        UserStatus userStatus
    );

    public Task DeleteUser(string auth0Id);
}
