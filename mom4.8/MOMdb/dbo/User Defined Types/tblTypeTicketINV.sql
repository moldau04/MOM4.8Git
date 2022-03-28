CREATE TYPE [dbo].[tblTypeTicketINV] AS TABLE(
	[Ticket] [int] NULL,
	[Line] [smallint] NULL,
	[Item] [int] NULL,
	[Quan] [numeric](30, 2) NULL,
	[fDesc] [varchar](255) NULL,
	[Charge] [smallint] NULL,
	[Amount] [numeric](30, 2) NULL,
	[Phase] [smallint] NULL,
	[AID] [varchar](100) NULL,
	[TypeID] [int] NULL,
	[WarehouseID] [varchar](50) NULL,
	[LocationID] [int] NULL,
	[PhaseName] [varchar](50) NULL
)
GO