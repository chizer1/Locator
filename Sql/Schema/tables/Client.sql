CREATE TABLE [dbo].[Client] (
   [ClientID] [int] NOT NULL
      IDENTITY (1,1),
   [ClientCode] [varchar](20) NOT NULL,
   [ClientName] [varchar](50) NOT NULL,
   [ClientStatusID] [int] NOT NULL,
   [CreateDate] [datetime2](2) NOT NULL,
   [CreateByID] [int] NOT NULL,
   [ModifyDate] [datetime2](2) NOT NULL,
   [ModifyByID] [int] NOT NULL,
   [RowGuid] [uniqueidentifier] NOT NULL

   ,CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED ([ClientID])
)


GO
