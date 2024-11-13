namespace Locator.Models.Read;

public class Client
{
    public int ClientId { get; init; }
    public string ClientName { get; set; }
    public string ClientCode { get; set; }
    public ClientStatus ClientStatus { get; set; }
}
