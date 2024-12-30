namespace Locator.Features.ClientUsers;

internal interface IClientUserRepository
{
    public Task<int> AddClientUser(int clientId, int userId);
    public Task DeleteClientUser(int clientId, int userId);
}
