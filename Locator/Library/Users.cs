using System.Data.SqlClient;
using Locator.Common;
using Locator.Common.Models;
using Locator.Domain;
using Locator.Features.Users;
using Locator.Features.Users.AddUser;
using Locator.Features.Users.DeleteUser;
using Locator.Features.Users.GetUsers;
using Locator.Features.Users.UpdateUser;

namespace Locator.Library;

public class Users
{
    private readonly AddUser _addUser;
    private readonly GetUsers _getUsers;
    private readonly UpdateUser _updateUser;
    private readonly DeleteUser _deleteUser;

    public Users(SqlConnection locatorDb, Auth0 auth0)
    {
        IUserRepository userRepository = new UserRepository(locatorDb);
        IAuth0UserService auth0UserService = new Auth0UserService(auth0);

        _addUser = new AddUser(userRepository, auth0UserService);
        _getUsers = new GetUsers(userRepository);
        _updateUser = new UpdateUser(userRepository, auth0UserService);
        _deleteUser = new DeleteUser(userRepository, auth0UserService);
    }

    public async Task<int> AddUser(
        string firstName,
        string lastName,
        string emailAddress,
        string password,
        UserStatus status
    )
    {
        return await _addUser.Handle(
            new AddUserCommand(firstName, lastName, emailAddress, password, status)
        );
    }

    public async Task<PagedList<User>> GetUsers(string keyword, int pageNumber, int pageSize)
    {
        return await _getUsers.Handle(new GetUsersQuery(keyword, pageNumber, pageSize));
    }

    public async Task UpdateUser(
        int userId,
        string firstName,
        string lastName,
        string emailAddress,
        string password,
        UserStatus status
    )
    {
        await _updateUser.Handle(
            new UpdateUserCommand(userId, firstName, lastName, emailAddress, password, status)
        );
    }

    public async Task DeleteUser(int userId)
    {
        await _deleteUser.Handle(new DeleteUserCommand(userId));
    }
}
