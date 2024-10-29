namespace Locator.Models.Read;

public class User
{
    public int UserId { get; set; }
    public string Auth0Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public UserStatus UserStatus { get; set; }
    public List<Role> Roles { get; set; }
}
