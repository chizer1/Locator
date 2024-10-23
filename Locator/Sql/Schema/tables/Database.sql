CREATE TABLE [dbo].[Database] (
   [DatabaseID] [int] NOT NULL
      IDENTITY (1,1),
   [DatabaseName] [varchar](50) NOT NULL,
   [DatabaseUser] [varchar](50) NOT NULL,
   [DatabaseUserPassword] [varchar](50) NOT NULL,
   [DatabaseServerID] [int] NOT NULL,
   [DatabaseTypeID] [tinyint] NOT NULL,
   [CreateDate] [datetime2] NOT NULL,
   [CreateByID] [int] NOT NULL,
   [ModifyDate] [datetime2] NOT NULL,
   [ModifyByID] [int] NOT NULL,
   [RowGuid] [uniqueidentifier] NOT NULL

   ,CONSTRAINT [PK_Database] PRIMARY KEY CLUSTERED ([DatabaseID])
)


GO
