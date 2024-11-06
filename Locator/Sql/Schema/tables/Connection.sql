CREATE TABLE [dbo].[Connection] (
   [ConnectionID] [int] NOT NULL
      IDENTITY (1,1),
   [ClientUserID] [int] NOT NULL,
   [DatabaseID] [int] NOT NULL

   ,CONSTRAINT [PK_ClientConnection] PRIMARY KEY CLUSTERED ([ConnectionID])
)

CREATE NONCLUSTERED INDEX [ix_ClientConnection_ClientID] ON [dbo].[Connection] ([ClientUserID])

GO
