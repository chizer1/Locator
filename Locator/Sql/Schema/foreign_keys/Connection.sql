ALTER TABLE [dbo].[Connection] WITH CHECK ADD CONSTRAINT [FK_Connection_ClientUser]
   FOREIGN KEY([ClientUserID]) REFERENCES [dbo].[ClientUser] ([ClientUserID])

GO
ALTER TABLE [dbo].[Connection] WITH CHECK ADD CONSTRAINT [FK_Connection_Database]
   FOREIGN KEY([DatabaseID]) REFERENCES [dbo].[Database] ([DatabaseID])

GO
