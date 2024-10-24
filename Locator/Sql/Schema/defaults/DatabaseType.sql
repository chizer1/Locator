ALTER TABLE [dbo].[DatabaseType] ADD CONSTRAINT [DEF_DatatbaseType_CreateDate] DEFAULT (getutcdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[DatabaseType] ADD CONSTRAINT [DEF_DatatbaseType_ModifyDate] DEFAULT (getutcdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[DatabaseType] ADD CONSTRAINT [DEF_DatatbaseType_RowGuid] DEFAULT (newid()) FOR [RowGuid]
GO
