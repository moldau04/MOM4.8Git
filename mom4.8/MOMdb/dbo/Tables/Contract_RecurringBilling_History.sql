CREATE TABLE [dbo].[Contract_RecurringBilling_History](
		[JobID] [int] NULL,
		[LastExeTime] [datetime] NULL,
		[bamt] [numeric](30,2) null,
		[BCycle] [INT]
	) ON [PRIMARY]