/*--------------------------------------------------------------------
Modified By: Thurstan
Modified On: 30 Nov 2018	
Description: Add Orderno column 
--------------------------------------------------------------------*/
CREATE TYPE [dbo].[tblTypeProjectItem] AS TABLE(
	[JobT] [int] NULL,
	[Job] [int] NULL,
	[Type] [smallint] NULL,
	[fDesc] [varchar](255) NULL,
	[Code] [varchar](10) NULL,
	[Actual] [numeric](30, 2) NULL,
	[Budget] [numeric](30, 2) NULL,
	[Line] [smallint] NULL,
	[Percent] [numeric](30, 2) NULL,
	[OrderNo] int NULL
)
GO


