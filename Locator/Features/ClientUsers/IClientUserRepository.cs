namespace Locator.Features.ClientUsers;

public interface IClientUserRepository
{
    public Task<int> AddClientUser(int clientId, int userId);
    public Task DeleteClientUser(int clientId, int userId);
}
