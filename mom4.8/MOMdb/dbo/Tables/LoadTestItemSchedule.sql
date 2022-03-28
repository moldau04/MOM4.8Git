CREATE TABLE [dbo].[LoadTestItemSchedule](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[LID] [int] NOT NULL,
	[ScheduledYear] [int] NOT NULL,
	[ScheduledDate] [varchar](max) NULL,
	[ScheduledStatus] [int] NULL,
	[Worker] [varchar](max) NULL,
	[CreatedBy] [varchar](100) NULL,
	[TicketId] INT,
	[TicketStatus] INT,
 CONSTRAINT [PK_LoadTestItemSchedule] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO