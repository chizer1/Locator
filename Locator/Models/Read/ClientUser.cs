namespace Locator.Models.Read;

public class ClientUser
{
    public int ClientUserId { get; set; }
    public Client Client { get; set; }
    public User User { get; set; }
}
