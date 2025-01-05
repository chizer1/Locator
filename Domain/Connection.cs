namespace Locator.Domain;

/// <summary>
/// Represents a connection to a database.
/// </summary>
/// <param name="id">The unique identifier for the connection.</param>
/// <param name="databaseId">The identifier of the database.</param>
/// <param name="clientUserId">The identifier of the client user.</param>
public class Connection(int id, int databaseId, int clientUserId)
{
    /// <summary>
    /// Gets the unique identifier for the connection.
    /// </summary>
    public int Id { get; init; } = id;

    /// <summary>
    /// Gets or sets the identifier of the database.
    /// </summary>
    public int DatabaseId { get; set; } = databaseId;

    /// <summary>
    /// Gets or sets the identifier of the client user.
    /// </summary>
    public int ClientUserId { get; set; } = clientUserId;
};
