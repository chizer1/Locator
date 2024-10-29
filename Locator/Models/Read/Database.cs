namespace Locator.Models.Read;

public class Database
{
    public int DatabaseId { get; set; }
    public string DatabaseName { get; set; }
    public string DatabaseUser { get; set; }
    public DatabaseType DatabaseType { get; set; }
}
