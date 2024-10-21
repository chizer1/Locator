CREATE TABLE [dbo].[DatabaseServer] (
   [DatabaseServerID] [int] NOT NULL
      IDENTITY (1,1),
   [DatabaseServerName] [varchar](50) NOT NULL,
   [CreateDate] [datetime2] NOT NULL,
   [CreateByID] [int] NOT NULL,
   [ModifyDate] [datetime2] NOT NULL,
   [ModifyByID] [int] NOT NULL,
   [RowGuid] [uniqueidentifier] NOT NULL

   ,CONSTRAINT [PK_DatabaseServer] PRIMARY KEY CLUSTERED ([DatabaseServerID])
)


GO
