namespace Locator.Models;

public class User
{
    public string Auth0Id { get; set; }
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public int UserStatusId { get; set; }
    public int ClientId { get; set; }
    public List<int> RoleIds { get; set; }
}
