CREATE TYPE [dbo].[tblTypeAlerts] AS TABLE(
	[AlertID] [int] NULL,
	[AlertCode] [varchar](50) NULL,
	[AlertName] [varchar](50) NULL,	
	[AlertSubject] [varchar](50) NULL,
	[AlertMessage] [text] NULL
)
GO