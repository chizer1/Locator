ALTER TABLE [dbo].[User] WITH CHECK ADD CONSTRAINT [FK_User_Client]
   FOREIGN KEY([ClientID]) REFERENCES [dbo].[Client] ([ClientID])

GO
ALTER TABLE [dbo].[User] WITH CHECK ADD CONSTRAINT [FK_User_UserStatus]
   FOREIGN KEY([UserStatusID]) REFERENCES [dbo].[UserStatus] ([UserStatusID])

GO
