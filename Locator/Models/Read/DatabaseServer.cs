namespace Locator.Models.Read;

public class DatabaseServer
{
    public int DatabaseServerId { get; set; }
    public string DatabaseServerName { get; set; }
    public string DatabaseServerIpAddress { get; set; }
    public List<Database> Databases { get; set; }
}
