ALTER TABLE [dbo].[UserRole] WITH CHECK ADD CONSTRAINT [FK_UserRole_Role]
   FOREIGN KEY([RoleID]) REFERENCES [dbo].[Role] ([RoleID])

GO
ALTER TABLE [dbo].[UserRole] WITH CHECK ADD CONSTRAINT [FK_UserRole_User]
   FOREIGN KEY([UserID]) REFERENCES [dbo].[User] ([UserID])

GO
