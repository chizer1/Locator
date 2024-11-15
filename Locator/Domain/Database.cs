namespace Locator.Domain;

public class Database
{
    public int DatabaseId { get; init; }
    public string DatabaseName { get; init; }
    public string DatabaseUserName { get; init; }
    public DatabaseType DatabaseType { get; set; }
}
