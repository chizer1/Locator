namespace Locator.Models;

public class Database
{
    public string DatabaseId { get; set; }
    public string DatabaseName { get; set; }
    public DatabaseServer DatabaseServer { get; set; }
    public DatabaseType DatabaseType { get; set; }
}
