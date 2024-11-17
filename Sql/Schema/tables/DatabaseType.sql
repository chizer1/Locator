CREATE TABLE [dbo].[DatabaseType] (
   [DatabaseTypeID] [tinyint] NOT NULL
      IDENTITY (1,1),
   [DatabaseTypeName] [varchar](20) NOT NULL

   ,CONSTRAINT [PK_DatabaseType] PRIMARY KEY CLUSTERED ([DatabaseTypeID])
)


GO
