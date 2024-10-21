SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
create trigger User_ClientID_Update_Prevention
on dbo.[User]
for update
as

begin

	raiserror('Access Denied', 16, 1)
	rollback transaction
	return

end
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

DISABLE TRIGGER [dbo].[User_ClientID_Update_Prevention] ON [dbo].[User]
GO

GO
