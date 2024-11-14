namespace Locator.Models.Read;

public class DatabaseServer
{
    public int DatabaseServerId { get; init; }
    public string DatabaseServerName { get; init; }
    public string DatabaseServerIpAddress { get; init; }
    public List<Database> Databases { get; set; }
}
