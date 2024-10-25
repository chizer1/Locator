using System.Data;

namespace Locator.Repositories;

internal class ConnectionRepository(IDbConnection locatorDb)
{
    public void Test()
    {
        Console.WriteLine(locatorDb);
    }
}
