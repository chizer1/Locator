namespace Locator.Domain;

public class Database(int id, string name, string user, int databaseServerId, Status status)
{
    public int Id { get; init; } = id;
    public string Name { get; init; } = name;
    public string User { get; set; } = user;
    public int DatabaseServerId { get; init; } = databaseServerId;
    public Status Status { get; set; } = status;
}
