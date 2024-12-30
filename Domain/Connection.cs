namespace Locator.Domain;

public class Connection(int id, int databaseId, int clientUserId)
{
    public int Id { get; init; } = id;
    public int DatabaseId { get; set; } = databaseId;
    public int ClientUserId { get; set; } = clientUserId;
};
