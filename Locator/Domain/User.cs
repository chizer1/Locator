namespace Locator.Domain;

public class User
{
    public int UserId { get; init; }
    public string Auth0Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string EmailAddress { get; init; }
    public UserStatus UserStatus { get; init; }
}
