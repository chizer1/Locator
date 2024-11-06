CREATE TABLE [dbo].[RolePermission] (
   [RolePermissionID] [int] NOT NULL
      IDENTITY (1,1),
   [RoleID] [int] NOT NULL,
   [PermissionID] [int] NOT NULL

   ,CONSTRAINT [PK_RolePermission] PRIMARY KEY CLUSTERED ([RolePermissionID])
)


GO
