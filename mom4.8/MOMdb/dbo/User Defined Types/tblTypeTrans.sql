CREATE TYPE [dbo].[tblTypeTrans] AS TABLE(
	[ID] [int] NULL,
	[AcctID] [int] NULL,
	[AcctNo] [varchar](100) NULL,
	[Account] [varchar](150) NULL,
	[fDesc] [varchar](max) NULL,
	[Debit] [numeric](30, 2) NULL,
	[Credit] [numeric](30, 2) NULL,
	[Loc] [varchar](150) NULL,
	[JobName] [varchar](max) NULL,
	[JobID] [int] NULL,
	[Phase] [varchar](250) NULL,
	[PhaseID] [int] NULL,
	[LocID] [int] NULL,
	[TypeID] [int] NULL
)





