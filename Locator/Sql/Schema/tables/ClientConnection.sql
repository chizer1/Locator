CREATE TABLE [dbo].[ClientConnection] (
   [ClientConnectionID] [int] NOT NULL
      IDENTITY (1,1),
   [ClientID] [int] NOT NULL,
   [DatabaseID] [int] NULL,
   [CreateDate] [datetime2](2) NOT NULL,
   [CreateByID] [int] NOT NULL,
   [ModifyDate] [datetime2](2) NOT NULL,
   [ModifyByID] [int] NOT NULL,
   [RowGuid] [uniqueidentifier] NOT NULL

   ,CONSTRAINT [PK_ClientConnection] PRIMARY KEY CLUSTERED ([ClientConnectionID])
)

CREATE NONCLUSTERED INDEX [ix_ClientConnection_ClientID] ON [dbo].[ClientConnection] ([ClientID])

GO
