using Locator.Domain;
using Locator.Models.Read;

namespace Locator.Features.Users;

public interface IUserRepository
{
    public Task<int> AddUser(
        string firstName,
        string lastName,
        string emailAddress,
        UserStatus userStatus,
        string auth0Id
    );

    public Task<User> GetUser(string emailAddress);

    public Task<User> GetUser(int userId);

    public Task<List<User>> GetUsers();

    public Task<PagedList<User>> GetUsers(string keyword, int pageNumber, int pageSize);

    public Task UpdateUser(
        int userId,
        string firstName,
        string lastName,
        string emailAddress,
        UserStatus userStatus
    );

    public Task DeleteUser(int userId);

    public Task DeleteUser(string auth0Id);
}
