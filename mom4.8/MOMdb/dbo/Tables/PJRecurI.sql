CREATE TABLE [dbo].[PJRecurI](
	[ID] [int] NULL,
	[AcctID] [int] NULL,
	[fDesc] [varchar](max) NULL,
	[Amount] [numeric](30, 4) NULL,
	[UseTax] [numeric](30, 4) NULL,
	[UtaxName] [varchar](25) NULL,
	[JobID] [int] NULL,
	[PhaseID] [int] NULL,
	[ItemID] [int] NULL,
	[Phase] [varchar](100) NULL,
	[UTaxGL] [int] NULL,
	[ItemDesc] [varchar](150) NULL,
	[TypeID] [int] NULL,
	[TypeDesc] [varchar](150) NULL,
	[Quan] [numeric](30, 4) NULL,
	[Ticket] [int] NULL,
	[OpSq] [varchar](150) NULL,
	[Warehouse] [varchar](50) NULL,
	[WHLocID] [int] NULL,
	[PJID] [int] NULL,
	[Line] [int] NULL
);

