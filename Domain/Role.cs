namespace Locator.Domain;

public class Role(int id, string auth0RoleId, string name, string description)
{
    public int Id { get; init; } = id;
    public string Auth0RoleId { get; init; } = auth0RoleId;
    public string Name { get; init; } = name;
    public string Description { get; init; } = description;
};
