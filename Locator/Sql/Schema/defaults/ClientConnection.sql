ALTER TABLE [dbo].[ClientConnection] ADD CONSTRAINT [DEF_ClientConnection_CreateDate] DEFAULT (getutcdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[ClientConnection] ADD CONSTRAINT [DEF_ClientConnection_ModifyDate] DEFAULT (getutcdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[ClientConnection] ADD CONSTRAINT [DEF_ClientConnection_RowGuid] DEFAULT (newid()) FOR [RowGuid]
GO
