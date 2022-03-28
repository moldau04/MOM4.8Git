/*--------------------------------------------------------------------
Modified By: Thurstan
Modified On: 30 Nov 2018	
Description: Add Orderno column
--------------------------------------------------------------------*/
CREATE TYPE [dbo].[tblTypeEstimateBomItem] AS TABLE(
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
	[MatItem] [int] NULL,
	[MatMod] [numeric](30, 2) NULL,
	[MatPrice] [numeric](30, 2) NULL,
	[MatMarkup] [numeric](30, 2) NULL,
	[STax] [tinyint] NULL,
	[Currency] [varchar](10) NULL,
	[LabItem] [int] NULL,
	[LabMod] [numeric](30, 2) NULL,
	[LabExt] [numeric](30, 2) NULL,
	[LabRate] [numeric](30, 2) NULL,
	[LabHours] [numeric](30, 2) NULL,
	[SDate] [datetime] NULL,
	[VendorId] [int] NULL,
	[Vendor] [varchar](75) NULL,
	[TotalExt] [numeric](30, 2) NULL,
	[LabPrice] [numeric](30, 2) NULL,
	[LabMarkup] [numeric](30, 2) NULL,
	[LSTax] [tinyint] NULL,
	[EstimateItemID] [int] NULL,
	[OrderNo] [int] NULL
)
GO


