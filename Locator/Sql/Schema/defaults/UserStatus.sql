ALTER TABLE [dbo].[UserStatus] ADD CONSTRAINT [DEF_UserStatus_CreateDate] DEFAULT (getutcdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[UserStatus] ADD CONSTRAINT [DEF_UserStatus_ModifyDate] DEFAULT (getutcdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[UserStatus] ADD CONSTRAINT [DEF_UserStatus_RowGuid] DEFAULT (newid()) FOR [RowGuid]
GO
