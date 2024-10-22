using Locator.Repositories;
using System;
using System.Data.SqlClient;
using System.Data;

IDbConnection dbConnection = new SqlConnection("Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;");

var LocatorRepository = new LocatorRepository(dbConnection);
//await LocatorRepository.GetClientList();

Console.WriteLine("Hello, World!");