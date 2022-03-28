CREATE TABLE [dbo].[tblPingDevice](
	[deviceID] [varchar](100) NOT NULL,
	[randomID] [varchar](100) NOT NULL,
	[date] [datetime] NULL,
	[IsRunning] [smallint] NULL,
	[IsGPSEnabled] [smallint] NULL,
	[BackgroundRefresh] [int] NULL,
	[FUser] [varchar](50) NULL,
	[UserId] [int] NULL
) ON [PRIMARY]
GO