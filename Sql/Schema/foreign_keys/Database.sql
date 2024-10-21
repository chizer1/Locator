ALTER TABLE [dbo].[Database] WITH CHECK ADD CONSTRAINT [FK_Database_DatabaseServer]
   FOREIGN KEY([DatabaseServerID]) REFERENCES [dbo].[DatabaseServer] ([DatabaseServerID])

GO
ALTER TABLE [dbo].[Database] WITH CHECK ADD CONSTRAINT [FK_Database_DatabaseType]
   FOREIGN KEY([DatabaseTypeID]) REFERENCES [dbo].[DatabaseType] ([DatabaseTypeID])

GO
