namespace Locator.Domain;

/// <summary>
/// Represents a permission with an ID, name, and description.
/// </summary>
/// <param name="id">The unique identifier for the permission.</param>
/// <param name="name">The name of the permission.</param>
/// <param name="description">The description of the permission.</param>
public class Permission(int id, string name, string description)
{
    /// <summary>
    /// Gets the unique identifier for the permission.
    /// </summary>
    public int Id { get; init; } = id;

    /// <summary>
    /// Gets or sets the name of the permission.
    /// </summary>
    public string Name { get; set; } = name;

    /// <summary>
    /// Gets or sets the description of the permission.
    /// </summary>
    public string Description { get; set; } = description;
}
