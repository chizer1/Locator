namespace Locator.Models;

public class DatabaseServer
{
    public string DatabaseServerId { get; set; }
    public string DatabaseServerName { get; set; }
    public string DatabaseServerIpAddress { get; set; }
    public List<Database> Databases { get; set; }
}
