ALTER TABLE [dbo].[UserRole] ADD CONSTRAINT [DEF_UserRole_CreateDate] DEFAULT (getutcdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[UserRole] ADD CONSTRAINT [DEF_UserRole_ModifyDate] DEFAULT (getutcdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[UserRole] ADD CONSTRAINT [DEF_UserRole_RowGuid] DEFAULT (newid()) FOR [RowGuid]
GO
