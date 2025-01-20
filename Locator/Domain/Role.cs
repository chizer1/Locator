namespace Locator.Domain;

/// <summary>
/// Represents a role with an ID, Auth0 role ID, name, and description.
/// </summary>
/// <param name="id">The unique identifier for the role.</param>
/// <param name="auth0RoleId">The Auth0 role identifier.</param>
/// <param name="name">The name of the role.</param>
/// <param name="description">The description of the role.</param>
public class Role(int id, string auth0RoleId, string name, string description)
{
    /// <summary>
    /// Gets the unique identifier for the role.
    /// </summary>
    public int Id { get; init; } = id;

    /// <summary>
    /// Gets the Auth0 role identifier.
    /// </summary>
    public string Auth0RoleId { get; init; } = auth0RoleId;

    /// <summary>
    /// Gets the name of the role.
    /// </summary>
    public string Name { get; init; } = name;

    /// <summary>
    /// Gets the description of the role.
    /// </summary>
    public string Description { get; init; } = description;
};
