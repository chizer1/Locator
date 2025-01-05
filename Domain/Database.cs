namespace Locator.Domain;

/// <summary>
/// Represents a database.
/// </summary>
/// <param name="id">The database ID.</param>
/// <param name="name">The name of the database.</param>
/// <param name="user">The user associated with the database.</param>
/// <param name="databaseServerId">The ID of the database server.</param>
/// <param name="status">The status of the database.</param>
public class Database(int id, string name, string user, int databaseServerId, Status status)
{
    /// <summary>
    /// Gets the database ID.
    /// </summary>
    public int Id { get; init; } = id;

    /// <summary>
    /// Gets the name of the database.
    /// </summary>
    public string Name { get; init; } = name;

    /// <summary>
    /// Gets or sets the user associated with the database.
    /// </summary>
    public string User { get; set; } = user;

    /// <summary>
    /// Gets the ID of the database server.
    /// </summary>
    public int DatabaseServerId { get; init; } = databaseServerId;

    /// <summary>
    /// Gets or sets the status of the database.
    /// </summary>
    public Status Status { get; set; } = status;
}
