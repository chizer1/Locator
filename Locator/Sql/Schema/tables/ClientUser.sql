CREATE TABLE [dbo].[ClientUser] (
   [ClientUserID] [int] NOT NULL
      IDENTITY (1,1),
   [ClientID] [int] NOT NULL,
   [UserID] [int] NOT NULL

   ,CONSTRAINT [PK_ClientUser] PRIMARY KEY CLUSTERED ([ClientUserID])
)


GO
