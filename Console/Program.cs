using System.Data;
using System.Data.SqlClient;
using Locator;

IDbConnection dbConnection = new SqlConnection(
    "Server=localhost;Database=Locator;User Id=sa;Password=1StrongPwd!!;"
);

LocatorX locator = new(dbConnection);
var clients = await locator.GetClients();

// print out the clients
foreach (var client in clients)
{
    Console.WriteLine($"{client.ClientId} - {client.ClientName} - {client.ClientCode}");
}