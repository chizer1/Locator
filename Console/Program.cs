using System.Data;
using System.Data.SqlClient;
using Locator;
using Locator.Models;
using Dapper;


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

// get a connection with auth0Id, clientId, and databaseType
SqlConnection connection = await locator.GetConnection(
    "b1774d4c-ad83-4c7d-a2f8-90937d08e212",
    2002,
    2
);

// create db connection with connection string and run a sample query with dapper
connection.Open();
var result = await connection.QueryAsync("select * from dbo.Stuff");
foreach (var row in result)
{
    Console.WriteLine(row);
}
connection.Close();