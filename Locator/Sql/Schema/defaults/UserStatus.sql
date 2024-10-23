ALTER TABLE [dbo].[UserStatus] ADD CONSTRAINT [DEF_UserStatus_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[UserStatus] ADD CONSTRAINT [DEF_UserStatus_ModifyDate] DEFAULT (getdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[UserStatus] ADD CONSTRAINT [DEF_UserStatus_RowGuid] DEFAULT (newid()) FOR [RowGuid]
GO
