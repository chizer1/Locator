CREATE TABLE [dbo].[DatabaseType] (
   [DatabaseTypeID] [tinyint] NOT NULL,
   [DatabaseTypeName] [varchar](10) NOT NULL,
   [CreateDate] [datetime2](2) NOT NULL,
   [CreateByID] [int] NOT NULL,
   [ModifyDate] [datetime2](2) NOT NULL,
   [ModifyByID] [int] NOT NULL,
   [RowGuid] [uniqueidentifier] NOT NULL

   ,CONSTRAINT [PK_DatabaseType] PRIMARY KEY CLUSTERED ([DatabaseTypeID])
)


GO
