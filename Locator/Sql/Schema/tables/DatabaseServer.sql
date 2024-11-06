CREATE TABLE [dbo].[DatabaseServer] (
   [DatabaseServerID] [int] NOT NULL
      IDENTITY (1,1),
   [DatabaseServerName] [varchar](50) NOT NULL,
   [DatabaseServerIPAddress] [varchar](50) NOT NULL

   ,CONSTRAINT [PK_DatabaseServer] PRIMARY KEY CLUSTERED ([DatabaseServerID])
)


GO
