namespace Locator.Domain;

public class DatabaseType(int id, string name)
{
    public int Id { get; init; } = id;
    public string Name { get; init; } = name;
}