ALTER TABLE [dbo].[DatabaseServer] ADD CONSTRAINT [DEF_DatabaseServer_CreateDate] DEFAULT (getutcdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[DatabaseServer] ADD CONSTRAINT [DEF_DatabaseServer_ModifyDate] DEFAULT (getutcdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[DatabaseServer] ADD CONSTRAINT [DEF_DatabaseServer_RowGuid] DEFAULT (newid()) FOR [RowGuid]
GO
