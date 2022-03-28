CREATE TABLE [dbo].[LeadEquip] (
	[ID] [int] IDENTITY(1, 1) NOT NULL
	,[Unit] [varchar](20) NULL
	,[State] [varchar](25) NULL
	,[Lead] [int] NULL
	,[Owner] [int] NULL
	,[Cat] [varchar](20) NULL
	,[Type] [varchar](20) NULL
	,[Building] [varchar](20) NULL
	,[Manuf] [varchar](50) NULL
	,[Remarks] [text] NULL
	,[Install] [datetime] NULL
	,[InstallBy] [varchar](25) NULL
	,[Since] [datetime] NULL
	,[Last] [datetime] NULL
	,[Price] [numeric](30, 2) NULL
	,[fGroup] [varchar](25) NULL
	,[fDesc] [varchar](50) NULL
	,[Serial] [varchar](50) NULL
	,[Template] [int] NULL
	,[Status] [smallint] NULL
	,[AID] UNIQUEIDENTIFIER CONSTRAINT [DF_LeadEquip_AID] DEFAULT (newid()) NULL
	,[Week] [varchar](50) NULL
	,[Category] [varchar](20) NULL
	,[CustomField] [varchar](100) NULL
	,[PrimarySyncID] [int] NULL
	,[LastUpdateDate] [datetime] NULL
	,[servicestart] [datetime] NULL
	,[schedulefreq] [varchar](50) NULL
	,[Route] [int] NULL
	,[assignedname] [varchar](50) NULL
	,[shut_down] [bit] NULL
	,[Classification] [varchar](20) NULL
	,ShutdownReason VARCHAR(max) NULL, 
    CONSTRAINT [PK_LeadEquip] PRIMARY KEY ([ID])
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

