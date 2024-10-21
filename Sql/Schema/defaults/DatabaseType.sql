ALTER TABLE [dbo].[DatabaseType] ADD CONSTRAINT [DEF_DatatbaseType_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[DatabaseType] ADD CONSTRAINT [DEF_DatatbaseType_ModifyDate] DEFAULT (getdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[DatabaseType] ADD CONSTRAINT [DEF_DatatbaseType_RowGuid] DEFAULT (newid()) FOR [RowGuid]
GO
