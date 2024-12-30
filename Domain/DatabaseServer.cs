namespace Locator.Domain;

public class DatabaseServer(int id, string name, string ipAddress)
{
    public int Id { get; init; } = id;
    public string Name { get; init; } = name;
    public string IpAddress { get; init; } = ipAddress;
};
