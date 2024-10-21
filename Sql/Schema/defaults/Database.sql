ALTER TABLE [dbo].[Database] ADD CONSTRAINT [DEFAULT_Database_CreateDate] DEFAULT (getutcdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Database] ADD CONSTRAINT [DEFAULT_NewTable_ModifyDate] DEFAULT (getutcdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[Database] ADD CONSTRAINT [DEFAULT_Database_RowGuid] DEFAULT (newid()) FOR [RowGuid]
GO
