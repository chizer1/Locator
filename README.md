# Locator Library

ABOUT

The purpose of the app is to help generate SQL server connections depending on Auth0 context as well user management + permissions / roles.

Folders from the root:

1. Locator - the class library that would be a reusable nuget package
2. API - minimal api implenting the methods from the Locator class library
3. Client - React app using auth0 npm package, calling the above minimal api

SETUP

How to setup your Auth0 tenant (or use what I saved already)

1. Create new tenant (aka Domain)
   - Auth0 Management API is automatically created (how to manage role, permissions, and users programtically)
2. Create new custom API 
   - Save the Id and Identifier for your new custom API
   - A machine to machine application will be automatically created (be able to)
   - Disable creating an account through Auth0, I will need be done programmatically through the library
3. Create machine to machine application and authorize your custom API and Auth0 Management API
4. Auth0 Management API to MachineToMachine needs the following permissions.
  - Users
    - create:users
    - update:users
    - read:users
    - delete:users
  - Roles
    - read:roles
    - create:roles
    - delete: roles
    - update: roles
  - Resource Server
    - update:resource_servers

  RBAC also needs to be enabled on custom api

  How to setup database

  1. For local development you can spin up a local SQL server instance with Docker 
      - Run `docker compose up` from root of repository
  2. Use SchemaZen to create the Locator database locally
      - `dotnet schemazen script --server localhost --u sa --p '1StrongPwd!!' --database 'Locator' --scriptDir '\Locator\SQL\Schema\'`
  3. Populate a couple of lookup tables
      - Execute `\Locator\SQL\Scripts\Seed.sql` on your newly created LocatorDB

GET BASIC EXAMPLE WORKING

- Start the API and React projects in different terminals
- Call these API endpoints in Swagger
  - Create a User 
  - Create a Client
  - Relate the User to the Client (ClientUser)
  - Create a Database Server (you should have running already, localhost)
  - Create a Database Type 
  - Create a Database connecting to your Database server and type previously created
  - Relate the ClientUser to this new Database (this says the user is allowed sql connections to the db)
  - Create a Role
  - Create a Permission
  - Relate a Role to a Permission (RolePermission)
  - Relate a User to a Role (Can this user access particular endpoints / pages based on their role permission?)

- Go to React app and sign in as the user you just created (may need to go Auth0 to reset password)
- Click call random api
  - Do you get 401, 403 or retrieve the correct data from the API you set up? Depends on how permissions are setup
