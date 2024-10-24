ALTER TABLE [dbo].[ClientStatus] ADD CONSTRAINT [DEF_ClientStatus_CreateDate] DEFAULT (getutcdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[ClientStatus] ADD CONSTRAINT [DEF_ClientStatus_ModifyDate] DEFAULT (getutcdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[ClientStatus] ADD CONSTRAINT [DEF_ClientStatus_RowGuid] DEFAULT (newid()) FOR [RowGuid]
GO
