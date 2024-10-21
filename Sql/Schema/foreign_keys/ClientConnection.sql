ALTER TABLE [dbo].[ClientConnection] WITH CHECK ADD CONSTRAINT [FK_ClientConnection_Client]
   FOREIGN KEY([ClientID]) REFERENCES [dbo].[Client] ([ClientID])

GO
ALTER TABLE [dbo].[ClientConnection] WITH CHECK ADD CONSTRAINT [FK_ClientConnection_Database]
   FOREIGN KEY([DatabaseID]) REFERENCES [dbo].[Database] ([DatabaseID])

GO
