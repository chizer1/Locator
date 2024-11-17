CREATE TABLE [dbo].[DatabaseStatus] (
   [DatabaseStatusID] [tinyint] NOT NULL
      IDENTITY (1,1),
   [DatabaseStatusName] [varchar](10) NOT NULL

   ,CONSTRAINT [PK_DatabaseStatus] PRIMARY KEY CLUSTERED ([DatabaseStatusID])
)


GO
