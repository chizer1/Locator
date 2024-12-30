# About

The purpose of the library is to help create SQL Server connections between multiple clients while also using Auth0 to manage users, roles and permissions.

Below is the database model used in this library.
![image](https://github.com/user-attachments/assets/4dbfe593-6b2c-4c49-92e5-de60aa01cf0c)

# Prerequisites

## Auth0 setup

1. Create tenant (https://auth0.com/docs/get-started/auth0-overview/create-tenants)
2. Create a new custom API (https://auth0.com/docs/get-started/auth0-overview/set-up-apis)
3. RBAC needs to be enabled on custom api
4. Create machine to machine application and authorize your custom API and Auth0 Management API (https://auth0.com/docs/get-started/auth0-overview/create-applications/machine-to-machine-apps)
5. Auth0 Management API to machine to machine needs the following permissions:
   1. Users
        - create:users
        - update:users
        - read:users
        - delete:users
   2. Roles
        - read:roles
        - create:roles
        - delete: roles
        - update: roles
   3. Resource Server
        - update:resource_servers

## Database setup

1. First, you need an instance of SQL Server running. For local development, you can either:
   - Use the SQL Server image in this repository by running `docker compose up` from the root. This requires Docker Desktop to be installed (https://docs.docker.com/get-started/get-docker/)
   - Install SQL Server directly on your machine (https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
2. Create the Locator database on your local SQL Server.
   - Run `dotnet ef database update` from the root of the repository.

# How to use / examples

After getting everything setup, you will now be able to initialize the base Locator object:
```
Locator.Locator locator =
    new(
        builder.Configuration["LocatorDb:ConnectionString"],
        builder.Configuration["Auth0:Domain"],
        builder.Configuration["Auth0:MachineToMachine:ClientID"],
        builder.Configuration["Auth0:MachineToMachine:ClientSecret"],
        builder.Configuration["Auth0:ApiIdentifier"],
        builder.Configuration["Auth0:ApiAudience"]
    );
```
It is up to you on how you want to store / retrieve these values.

- Documentation: https://chizer1.github.io/Locator/
- Example repository implementing this class library: https://github.com/chizer1/Locator-Example
