CREATE TABLE [dbo].[Client] (
   [ClientID] [int] NOT NULL
      IDENTITY (1,1),
   [ClientName] [varchar](50) NOT NULL,
   [ClientCode] [varchar](20) NOT NULL,
   [ClientStatusID] [int] NOT NULL

   ,CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED ([ClientID])
)


GO
