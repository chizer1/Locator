ALTER TABLE [dbo].[Role] ADD CONSTRAINT [DEF_Role_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Role] ADD CONSTRAINT [DEF_Role_ModifyDate] DEFAULT (getdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[Role] ADD CONSTRAINT [DEF_Role_RowGuid] DEFAULT (newid()) FOR [RowGuid]
GO
