namespace Locator.Domain;

/// <summary>
/// Represents a user in the system.
/// </summary>
public class User(
    int id,
    string auth0Id,
    string firstName,
    string lastName,
    string emailAddress,
    Status userStatus
)
{
    /// <summary>
    /// Gets the user ID.
    /// </summary>
    public int Id { get; init; } = id;

    /// <summary>
    /// Gets the Auth0 ID.
    /// </summary>
    public string Auth0Id { get; init; } = auth0Id;

    /// <summary>
    /// Gets the first name.
    /// </summary>
    public string FirstName { get; init; } = firstName;

    /// <summary>
    /// Gets the last name.
    /// </summary>
    public string LastName { get; init; } = lastName;

    /// <summary>
    /// Gets the email address.
    /// </summary>
    public string EmailAddress { get; init; } = emailAddress;

    /// <summary>
    /// Gets the user status.
    /// </summary>
    public Status Status { get; init; } = userStatus;
}
