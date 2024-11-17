CREATE TABLE [dbo].[Permission] (
   [PermissionID] [int] NOT NULL
      IDENTITY (1,1),
   [PermissionName] [varchar](50) NOT NULL,
   [PermissionDescription] [varchar](100) NOT NULL

   ,CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED ([PermissionID])
)


GO
