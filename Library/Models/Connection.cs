namespace Locator.Models;

public class Connection
{
    public int ClientId { get; set; }
    public string ClientCode { get; set; }
    public int UserId { get; set; }
    public string DatabaseServer { get; set; }
    public string DatabaseName { get; set; }
    public string DatabaseUser { get; set; }
    public string DatabaseUserPassword { get; set; }
}
