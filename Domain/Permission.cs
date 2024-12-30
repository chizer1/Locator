namespace Locator.Domain;

public class Permission(int id, string name, string description)
{
    public int Id { get; init; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
}
