namespace Locator.Models.Read;

public class Connection
{
    public int ConnectionId { get; init; }
    public Database Database { get; set; }
    public Client Client { get; set; }
}
