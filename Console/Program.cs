using System.Data;
using System.Data.SqlClient;
using Locator;
using Locator.Models;

IDbConnection dbConnection = new SqlConnection(
    "Server=localhost;Database=Locator;User Id=sa;Password=1StrongPwd!!;"
);

LocatorLib locator =
    new(
        dbConnection,
        "https://dev-xshhwrh4f1vis6lb.us.auth0.com/",
        "RCbDL6LnErLJfuXz1s3hvLf6bVJklNFl",
        "auY2mDwrU8dPwlU8cMI4iLd2q4Lsu5nvhsF60DCRWk542AadNTQ8lV_jHp0MzRAF"
    );

SqlConnection connection = await locator.GetConnection(
    "b1774d4c-ad83-4c7d-a2f8-90937d08e212",
    2002,
    2
);

// delete user
var user = await locator.GetUser("b1774d4c-ad83-4c7d-a2f8-90937d08e212");
Console.WriteLine(user);


// add databasvare
// var databaseId = await locator.AddDatabase(
//     "TestDB3",
//     "TestUser3",
//     Guid.NewGuid().ToString(),
//     1003,
//     2,
//     DatabaseStatus.Active
// );
// Console.WriteLine(databaseId);

// 2004 database

// add client database
// await locator.AddClientDatabase(2002, 2004);
