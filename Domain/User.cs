namespace Locator.Domain;

public class User(
    int id,
    string auth0Id,
    string firstName,
    string lastName,
    string emailAddress,
    Status userStatus
)
{
    public int Id { get; init; } = id;
    public string Auth0Id { get; init; } = auth0Id;
    public string FirstName { get; init; } = firstName;
    public string LastName { get; init; } = lastName;
    public string EmailAddress { get; init; } = emailAddress;
    public Status Status { get; init; } = userStatus;
}
