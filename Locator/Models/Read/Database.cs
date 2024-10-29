namespace Locator.Models.Read;

public class Database
{
    public int DatabaseId { get; set; }
    public string DatabaseName { get; set; }
    public string DatabaseUserName { get; set; }
    public DatabaseType DatabaseType { get; set; }
}
