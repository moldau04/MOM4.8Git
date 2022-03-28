CREATE TABLE [dbo].[LoadTestItemHistory](
	[LID] [int] NULL,
	[JobId] INT NULL,
	[TestYear] [int] NULL,
	[TestStatus] [int] NULL,
	[Last] [DATETIME] NULL,
	[Next] [DATETIME] NULL,
	[LastDue] [DATETIME],
	[TicketID] int NULL,	
	[TicketStatus] [int] NULL,
	[Worker] VARCHAR (100) NULL,
	[fWork] INT NULL,
	[Who] VARCHAR(100) NULL,
	[Schedule] VARCHAR (100) NULL,
	[UpdatedBy] VARCHAR(200) NULL,
	[isTestDefault] INT, 
	[IsActive] Int
) ON [PRIMARY]
GO
