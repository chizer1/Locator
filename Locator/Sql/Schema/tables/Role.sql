CREATE TABLE [dbo].[Role] (
   [RoleID] [int] NOT NULL
      IDENTITY (1,1),
   [Auth0RoleID] [varchar](20) NOT NULL,
   [Name] [varchar](20) NOT NULL,
   [Description] [varchar](50) NOT NULL

   ,CONSTRAINT [PK_Role_RoleID] PRIMARY KEY CLUSTERED ([RoleID])
)


GO
