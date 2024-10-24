using System.Data;
using System.Data.SqlClient;
using Locator;

IDbConnection dbConnection = new SqlConnection(
    "Server=localhost;Database=Locator;User Id=sa;Password=1StrongPwd!!;"
);

LocatorLib locator =
    new(
        dbConnection,
        "dev-xshhwrh4f1vis6lb.us.auth0.com",
        "auY2mDwrU8dPwlU8cMI4iLd2q4Lsu5nvhsF60DCRWk542AadNTQ8lV_jHp0MzRAF",
        "RCbDL6LnErLJfuXz1s3hvLf6bVJklNFl"
    );
    
var clients = await locator.GetClients();
foreach (var client in clients)
{
    Console.WriteLine($"{client.ClientId} - {client.ClientName} - {client.ClientCode}");
}