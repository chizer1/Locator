ALTER TABLE [dbo].[User] ADD CONSTRAINT [DEF_User_UserGuid] DEFAULT (newid()) FOR [UserGuid]
GO
ALTER TABLE [dbo].[User] ADD CONSTRAINT [DEF_User_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[User] ADD CONSTRAINT [DEF_User_ModifyDate] DEFAULT (getdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[User] ADD CONSTRAINT [DEF_User_RowGuid] DEFAULT (newid()) FOR [RowGuid]
GO
