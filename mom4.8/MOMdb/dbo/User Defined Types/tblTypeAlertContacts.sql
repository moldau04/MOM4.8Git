CREATE TYPE [dbo].[tblTypeAlertContacts] AS TABLE(
	[ID] [int] NULL,
	[ScreenID] [int] NULL,
	[ScreenName] [varchar](50) NULL,
	[AlertID] [int] NULL,
	[Name] [varchar](50) NULL,
	[Email] [bit] NULL,
	[Text] [bit] NULL,
	[AlertCode] varchar(50) null
)
