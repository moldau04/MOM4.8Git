/*--------------------------------------------------------------------
Modified By: Thurstan
Modified On: 30 Nov 2018	
Description: Add Orderno column
--------------------------------------------------------------------*/
CREATE TYPE [dbo].[tblTypeMilestoneItem] AS TABLE(
	[JobT] [int] NULL,
	[Job] [int] NULL,
	[jobTItem] [int] NULL,
	[jType] [smallint] NULL,
	[fdesc] [varchar](255) NULL,
	[jCode] [varchar](10) NULL,
	[Line] [int] NULL,
	[MilesName] [varchar](150) NULL,
	[RequiredBy] [datetime] NULL,
	[LeadTime] [numeric](30, 0) NULL,
	[ProjAcquistDate] [datetime] NULL,
	[ActAcquistDate] [datetime] NULL,
	[Comments] [varchar](max) NULL,
	[Type] [int] NULL,
	[Amount] [numeric](30, 2) NULL,
	[OrderNo] [int] NULL,
	[GroupID] [int] NULL, 
	[Quantity] [numeric](30, 2) NULL,
	[Price] [numeric](30, 2) NULL,
	[ChangeOrder] [tinyint] NULL
)
GO


