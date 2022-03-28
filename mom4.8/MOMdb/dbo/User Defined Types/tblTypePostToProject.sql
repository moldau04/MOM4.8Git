CREATE TYPE [dbo].[tblTypePostToProject] AS TABLE(
	[PhaseID] [int] NULL,
	[Phase] [varchar](100) NULL,
	[Inv] [int] NULL,
	[ItemDesc] [varchar](150) NULL,
	[fDesc] [varchar](max) NULL,
	[Quan] [numeric](30, 4) NULL,
	[WHLocID] [int] NULL,
	[WarehouseID] [varchar](50) NULL,
	[TypeID] [int] NULL,
	[JobID] [int] NULL,
	[AcctID] [int] NULL,
	[AcctNo] [varchar](255) NULL,
	[SchDate] DateTime NULL,
	[Worker] varchar(255),
	[Billed] int null
)