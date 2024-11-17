ALTER TABLE [dbo].[RolePermission] WITH CHECK ADD CONSTRAINT [FK_RolePermission_Permission]
   FOREIGN KEY([PermissionID]) REFERENCES [dbo].[Permission] ([PermissionID])

GO
ALTER TABLE [dbo].[RolePermission] WITH CHECK ADD CONSTRAINT [FK_RolePermission_Role]
   FOREIGN KEY([RoleID]) REFERENCES [dbo].[Role] ([RoleID])

GO
