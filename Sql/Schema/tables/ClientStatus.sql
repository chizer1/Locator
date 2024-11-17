CREATE TABLE [dbo].[ClientStatus] (
   [ClientStatusID] [int] NOT NULL
      IDENTITY (1,1),
   [ClientStatusName] [varchar](10) NOT NULL

   ,CONSTRAINT [PK_ClientStatus] PRIMARY KEY CLUSTERED ([ClientStatusID])
)


GO
