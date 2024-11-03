# Locator

How to setup Auth0

1. Create new tenant (aka Domain)
   - Auth0 Management API is automatically created (how to manage role, permissions, and users programtically)
2. Create new custom API 
   - Save the Id and Identifier for your new custom API
   - A machine to machine application will be automatically created (be able to)
   - Disable creating an account through Auth0, I will need be done programmatically through the library
3. Create machine to machine application and authorize your custom API and Auth0 Management API
    "MachineToMachine": {
      "ClientID": "4kCncUZGyfD1D5XklmKz1JbltbimEMQi",
      "ClientSecret": "253HfESIxduRwNHi7YeK4dpRIxlEKlLj_ZBLpMYG7pEP_iDqrUqNgZroFhY8VrJ4"
    }
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