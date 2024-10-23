ALTER TABLE [dbo].[UserRole] ADD CONSTRAINT [DEF_UserRole_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[UserRole] ADD CONSTRAINT [DEF_UserRole_ModifyDate] DEFAULT (getdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[UserRole] ADD CONSTRAINT [DEF_UserRole_RowGuid] DEFAULT (newid()) FOR [RowGuid]
GO
