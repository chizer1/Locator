ALTER TABLE [dbo].[ClientUser] WITH CHECK ADD CONSTRAINT [FK_ClientUser_Client]
   FOREIGN KEY([ClientID]) REFERENCES [dbo].[Client] ([ClientID])

GO
ALTER TABLE [dbo].[ClientUser] WITH CHECK ADD CONSTRAINT [FK_ClientUser_User]
   FOREIGN KEY([UserID]) REFERENCES [dbo].[User] ([UserID])

GO
