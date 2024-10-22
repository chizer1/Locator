CREATE TABLE [dbo].[User] (
   [UserID] [int] NOT NULL
      IDENTITY (1,1),
   [Auth0ID] [varchar](50) NOT NULL,
   [ClientID] [int] NOT NULL,
   [FirstName] [varchar](50) NOT NULL,
   [LastName] [varchar](50) NOT NULL,
   [EmailAddress] [varchar](100) NOT NULL,
   [UserStatusID] [tinyint] NOT NULL,
   [UserGuid] [uniqueidentifier] NOT NULL,
   [CreateDate] [datetime2](2) NOT NULL,
   [CreateByID] [int] NOT NULL,
   [ModifyDate] [datetime2](2) NOT NULL,
   [ModifyByID] [int] NOT NULL,
   [RowGuid] [uniqueidentifier] NOT NULL

   ,CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserID])
)

CREATE UNIQUE NONCLUSTERED INDEX [ix_User_Auth0ID] ON [dbo].[User] ([Auth0ID])

GO
