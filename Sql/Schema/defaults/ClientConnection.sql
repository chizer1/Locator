ALTER TABLE [dbo].[ClientConnection] ADD CONSTRAINT [DEF_ClientConnection_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[ClientConnection] ADD CONSTRAINT [DEF_ClientConnection_ModifyDate] DEFAULT (getdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[ClientConnection] ADD CONSTRAINT [DEF_ClientConnection_RowGuid] DEFAULT (newid()) FOR [RowGuid]
GO
