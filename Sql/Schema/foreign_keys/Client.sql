ALTER TABLE [dbo].[Client] WITH CHECK ADD CONSTRAINT [FK_Client_ClientStatus]
   FOREIGN KEY([ClientStatusID]) REFERENCES [dbo].[ClientStatus] ([ClientStatusID])

GO
