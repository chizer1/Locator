namespace Locator.Models.Read;

public class Client
{
    public int ClientId { get; init; }
    public string ClientName { get; init; }
    public string ClientCode { get; init; }
    public ClientStatus ClientStatus { get; init; }
}
