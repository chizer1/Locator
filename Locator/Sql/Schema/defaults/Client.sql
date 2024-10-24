ALTER TABLE [dbo].[Client] ADD CONSTRAINT [DEF_Client_CreateDate] DEFAULT (getutcdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Client] ADD CONSTRAINT [DEF_Client_ModifyDate] DEFAULT (getutcdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[Client] ADD CONSTRAINT [DEF_Client_RowGuid] DEFAULT (newid()) FOR [RowGuid]
GO
