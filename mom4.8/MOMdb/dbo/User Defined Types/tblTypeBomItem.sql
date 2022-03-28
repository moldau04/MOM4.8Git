/*--------------------------------------------------------------------
Modified By: Thurstan
Modified On: 30 Nov 2018	
Description: Add Orderno column 
--------------------------------------------------------------------*/

CREATE TYPE [dbo].[tblTypeBomItem] AS TABLE(
	[JobT] [int] NULL,
	[Job] [int] NULL,
	[JobTItemID] [int] NULL,
	[fDesc] [varchar](255) NULL,
	[Code] [varchar](10) NULL,
	[Line] [int] NULL,
	[BType] [smallint] NULL,
	[QtyReq] [numeric](30, 2) NULL,
	[UM] [varchar](50) NULL,
	[BudgetUnit] [numeric](30, 2) NULL,
	[BudgetExt] [numeric](30, 2) NULL,
	[LabItem] [int] NULL,
	[MatItem] [int] NULL,
	[MatMod] [numeric](30, 2) NULL,
	[LabMod] [numeric](30, 2) NULL,
	[LabExt] [numeric](30, 2) NULL,
	[LabRate] [numeric](30, 2) NULL,
	[LabHours] [numeric](30, 2) NULL,
	[SDate] [datetime] NULL,
	[VendorId] [int] NULL,
	[Vendor] [varchar](75) NULL,
	[TotalExt] [numeric](30, 2) NULL,
	[MatDesc] [varchar](75) NULL,
	[OrderNo] [int] NULL,
	[GroupID] [int] NULL,
	[STax] [tinyint] NULL,
	[LSTax] [tinyint] NULL
)
GO


