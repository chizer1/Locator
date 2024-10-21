ALTER TABLE [dbo].[DatabaseServer] ADD CONSTRAINT [DEF_DatabaseServer_CreateDate] DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[DatabaseServer] ADD CONSTRAINT [DEF_DatabaseServer_ModifyDate] DEFAULT (getdate()) FOR [ModifyDate]
GO
ALTER TABLE [dbo].[DatabaseServer] ADD CONSTRAINT [DEF_DatabaseServer_RowGuid] DEFAULT (newid()) FOR [RowGuid]
GO
