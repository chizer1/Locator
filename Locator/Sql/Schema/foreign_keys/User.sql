ALTER TABLE [dbo].[User] WITH CHECK ADD CONSTRAINT [FK_User_UserStatus]
   FOREIGN KEY([UserStatusID]) REFERENCES [dbo].[UserStatus] ([UserStatusID])

GO
