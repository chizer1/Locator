using System;
using System.Data;
using System.Data.SqlClient;
using Locator.Models;
using Locator.Repositories;

IDbConnection dbConnection = new SqlConnection(
    "Server=localhost;Database=Locator;User Id=sa;Password=1StrongPwd!!;"
);

var locatorRepo = new LocatorRepository(dbConnection);
var clientId = await locatorRepo.AddClient("Tricom Technical Services", "tricom", 1);

Console.WriteLine(clientId);
