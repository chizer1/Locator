namespace Locator.Domain;

/// <summary>
/// Represents a client with an ID, name, code, and status.
/// </summary>
public class Client(int id, string name, string code, Status status)
{
    /// <summary>
    /// Gets the ID of the client.
    /// </summary>
    public int Id { get; init; } = id;

    /// <summary>
    /// Gets the name of the client.
    /// </summary>
    public string Name { get; init; } = name;

    /// <summary>
    /// Gets the code of the client.
    /// </summary>
    public string Code { get; init; } = code;

    /// <summary>
    /// Gets the status of the client.
    /// </summary>
    public Status Status { get; init; } = status;
};
