namespace Locator.Domain;

public class Client(int id, string name, string code, Status status)
{
    public int Id { get; init; } = id;
    public string Name { get; init; } = name;
    public string Code { get; init; } = code;
    public Status Status { get; init; } = status;
};
