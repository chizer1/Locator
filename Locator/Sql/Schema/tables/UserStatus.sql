CREATE TABLE [dbo].[UserStatus] (
   [UserStatusID] [tinyint] NOT NULL
      IDENTITY (1,1),
   [UserStatusName] [varchar](10) NOT NULL

   ,CONSTRAINT [PK_UserStatus] PRIMARY KEY CLUSTERED ([UserStatusID])
)


GO
