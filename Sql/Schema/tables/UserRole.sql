CREATE TABLE [dbo].[UserRole] (
   [UserRoleID] [int] NOT NULL
      IDENTITY (1,1),
   [UserID] [int] NOT NULL,
   [RoleID] [int] NOT NULL

   ,CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([UserRoleID])
)

CREATE UNIQUE NONCLUSTERED INDEX [uix_UserRole_UserID_RoleID] ON [dbo].[UserRole] ([UserID], [RoleID])

GO
