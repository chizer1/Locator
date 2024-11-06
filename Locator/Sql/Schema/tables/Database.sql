CREATE TABLE [dbo].[Database] (
   [DatabaseID] [int] NOT NULL
      IDENTITY (1,1),
   [DatabaseName] [varchar](50) NOT NULL,
   [DatabaseUser] [varchar](50) NOT NULL,
   [DatabaseServerID] [int] NOT NULL,
   [DatabaseTypeID] [tinyint] NOT NULL,
   [DatabaseStatusID] [tinyint] NOT NULL

   ,CONSTRAINT [PK_Database] PRIMARY KEY CLUSTERED ([DatabaseID])
)


GO
