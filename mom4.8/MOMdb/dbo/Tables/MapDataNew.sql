CREATE TABLE [dbo].[MapDataNew](
	[deviceId] [varchar](100) NULL,
	[latitude] [varchar](50) NULL,
	[longitude] [varchar](50) NULL,
	[date] [datetime] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SysDate] [datetime] NULL,
	[fake] [int] NULL,
	[Accuracy] [varchar](50) NULL,
	[fuser] [varchar](50) NULL,
	[userId] [varchar](50) NULL,
	[battery] VARCHAR(255)NULL,
	[speed] VARCHAR(255)NULL
 CONSTRAINT [PK_MapDataNew] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
  	[timeStampType] [int] NULL,
	[category] [nvarchar](100) NULL,
	[ticketid] [int] NULL,
	[locname] [nvarchar](100) NULL,
) ON [PRIMARY]
