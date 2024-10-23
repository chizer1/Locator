ALTER TABLE [dbo].[ClientStatus] ADD CONSTRAINT [DEF_ClientStatus_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[ClientStatus] ADD CONSTRAINT [DEF_ClientStatus_ModifyDate] DEFAULT (getdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[ClientStatus] ADD CONSTRAINT [DEF_ClientStatus_RowGuid] DEFAULT (newid()) FOR [RowGuid]
GO
