using Locator.Common.Models;
using Locator.Domain;

namespace Locator.Features.Users;

internal interface IUserRepository
{
    public Task<int> AddUser(
        string firstName,
        string lastName,
        string emailAddress,
        Status userStatus,
        string auth0Id
    );
    public Task<User> GetUser(string emailAddress);
    public Task<User> GetUserByAuth0Id(string auth0Id);
    public Task<User> GetUser(int userId);
    public Task<List<User>> GetUsers();
    public Task<PagedList<User>> GetUsers(string keyword, int pageNumber, int pageSize);
    public Task UpdateUser(
        int userId,
        string firstName,
        string lastName,
        string emailAddress,
        Status userStatus
    );
    public Task DeleteUser(int userId);
    public Task DeleteUser(string auth0Id);
}
